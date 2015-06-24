using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Data.Constants;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class PagedQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IPredicateBuilder _predicateBuilder;

        public PagedQueryBuilderStrategy(IFieldHelper fieldHelper, IPredicateBuilder predicateBuilder) 
            : base(fieldHelper)
        {
            _predicateBuilder = Guard.EnsureIsNotNull("predicateBuilder", predicateBuilder);
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters = null) where TValue : class
        {
            PagingInfo pagingInfo = parameters.PagingInfo;
            IEnumerable<string> desiredFields = parameters.DesiredFields;
            Expression<Func<TValue, bool>> predicate = parameters.Predicate;
            bool canDirtyRead = parameters.CanDirtyRead;
            bool includeParameters = parameters.IncludeParameters;

            var queryBuilder = new StringBuilder();
            var fields = FieldHelper.BuildFields<TValue>(desiredFields);
            var orderByClause = BuildPagedOrderByClause<TValue>();
            var countClause = BuildCountClause<TValue>();
            var whereClause = String.Empty;
            var databaseParameters = new List<IDbDataParameter>();

            if (predicate.IsNotNull())
            {
                var container = _predicateBuilder.BuildContainer(predicate, typeof(TValue), includeParameters);

                whereClause = container.WhereClause;

                if (includeParameters)
                {
                    databaseParameters.AddRange(container.Parameters.ToSafeList());
                }
            }

            queryBuilder.Append("SET @sortColumn = LOWER(@sortColumn); ");
            queryBuilder.Append("SET @sortOrder = LOWER(@sortOrder); ");
            queryBuilder.Append(BuildCommonTableExpression(fields, orderByClause, countClause, canDirtyRead, whereClause));
            queryBuilder.Append("SELECT ");
            queryBuilder.Append(fields.FieldMappings
                .Select(f => f.Value.FieldName)
                .ToDelimitedString(", "));
            queryBuilder.Append(", ");
            queryBuilder.Append("[TotalRecords], ");
            queryBuilder.Append("([TotalRecords] + @rowsPerPage - 1) / @rowsPerPage AS NumberOfPages ");
            queryBuilder.AppendFormat("FROM {0} ", ItemNames.SortedPageCteName);
            queryBuilder.AppendFormat(
                "WHERE {0}.RowNumber BETWEEN ((@pageNumber - 1) * @rowsPerPage) + 1 AND @rowsPerPage * @pageNumber ", ItemNames.SortedPageCteName);

            if (whereClause.IsNotNullOrEmpty())
            {
                queryBuilder.Append(" ");
                queryBuilder.Append("AND {0} ".FormatString(whereClause));
            }

            queryBuilder.Append(orderByClause);
            queryBuilder.Append(";");

            databaseParameters.Add(new SqlParameter(Parameters.RowsPerPage, SqlDbType.Int) { Value = pagingInfo.RowsPerPage });
            databaseParameters.Add(new SqlParameter(Parameters.PageNumber, SqlDbType.Int) { Value = pagingInfo.PageNumber });
            databaseParameters.Add(new SqlParameter(Parameters.SortColumn, SqlDbType.NVarChar) { Value = pagingInfo.SortColumn });
            databaseParameters.Add(new SqlParameter(Parameters.SortOrder, SqlDbType.NVarChar) { Value = pagingInfo.SortOrder });

            return new QueryInfo(queryBuilder.ToString().Trim(), fields, databaseParameters);
        }

        protected string BuildPagedOrderByClause<TValue>()
        {
            var attributes = typeof(TValue).GetProperties()
                .SafeSelect(property => property.GetCustomAttributes(typeof(SortMetadataAttribute), true)
                    .SafeFirstOrDefault() as SortMetadataAttribute)
                .SafeWhere(sortAttribute => !sortAttribute.IsNull())
                .ToSafeList();

            if (attributes.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot build query.  No sort attributes found on Type {0}."
                    .FormatString(typeof(TValue).ToString()));
            }

            var clauseBuilder = new StringBuilder();

            clauseBuilder.Append("ORDER BY ");

            for (var index = 0; index < attributes.Count; index++)
            {
                var attribute = attributes[index];

                clauseBuilder.Append("CASE ");
                clauseBuilder.AppendFormat("WHEN @sortOrder <> 'desc' THEN {0} ", attribute.IsPrimitive ? "0" : "NULL");
                clauseBuilder.AppendFormat("WHEN @sortColumn = '{0}' THEN {1} ", attribute.SortColumn, attribute.Field);
                clauseBuilder.Append("END DESC, ");
                clauseBuilder.Append("CASE ");
                clauseBuilder.AppendFormat("WHEN @sortOrder <> 'asc' THEN {0} ", attribute.IsPrimitive ? "0" : "NULL");
                clauseBuilder.AppendFormat("WHEN @sortColumn = '{0}' THEN {1} ", attribute.SortColumn, attribute.Field);
                clauseBuilder.Append("END ASC");

                if (index < attributes.Count - 1)
                {
                    clauseBuilder.Append(",");
                }

                clauseBuilder.Append(" ");
            }

            return clauseBuilder.ToString();
        }

        protected string BuildCommonTableExpression(
            TableObjectMapping tableFieldInfo, string orderByClause,
            string countClause, bool allowDirtyRead = false, string whereClause = null)
        {
            var queryBuilder = new StringBuilder();

            queryBuilder.AppendFormat("WITH {0} AS ", ItemNames.SortedPageCteName);
            queryBuilder.Append("( ");
            queryBuilder.Append("SELECT ");
            queryBuilder.Append(tableFieldInfo
                .FieldMappings
                .Select(f => f.Value.FieldName)
                .ToDelimitedString(", "));
            queryBuilder.Append(", ");
            queryBuilder.Append("ROW_NUMBER() ");
            queryBuilder.Append("OVER ( ");
            queryBuilder.Append(orderByClause);
            queryBuilder.Append(" ");
            queryBuilder.Append(") AS RowNumber, ");
            queryBuilder.Append(countClause);
            queryBuilder.AppendFormat("FROM {0} ", tableFieldInfo.TableName);

            if (allowDirtyRead)
            {
                queryBuilder.Append("(NOLOCK) ");
            }

            if (whereClause.IsNotNullOrEmpty())
            {
                queryBuilder.AppendFormat("WHERE {0} ", whereClause);
            }

            queryBuilder.Append(") ");

            return queryBuilder.ToString();
        }

        protected string BuildCountClause<TValue>()
        {
            CountMetadataAttribute countAttribute = null;
            var found = false;

            foreach (var property in typeof(TValue).GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(typeof(CountMetadataAttribute), true))
                {
                    countAttribute = attribute as CountMetadataAttribute;
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }

            if (countAttribute.IsNull())
            {
                throw new InvalidOperationException("Cannot build query.  Type {0} has no count attribute."
                    .FormatString(typeof(TValue).ToString()));
            }

            return "COUNT({0}) OVER() AS TotalRecords ".FormatString(countAttribute.FieldName);
        }
    }
}
