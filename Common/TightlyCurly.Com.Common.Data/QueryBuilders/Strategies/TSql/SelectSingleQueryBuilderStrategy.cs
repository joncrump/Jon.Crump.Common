using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class SelectSingleQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IPredicateBuilder _predicateBuilder;

        public SelectSingleQueryBuilderStrategy(IFieldHelper fieldHelper, IPredicateBuilder predicateBuilder) 
            : base(fieldHelper)
        {
            _predicateBuilder = Guard.EnsureIsNotNull("predicateBuilder", predicateBuilder);
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters = null) where TValue : class
        {
            Expression<Func<TValue, bool>> predicate = parameters.Predicate;
            bool canDirtyRead = parameters.CanDirtyRead; 
            bool includeParameters = parameters.IncludeParameters;
            IEnumerable<string> desiredFields = parameters.DesiredFields;
            string tableName = parameters.TableName;

            var fields = FieldHelper.BuildFields<TValue>(desiredFields, tableName);

            var queryBuilder = new StringBuilder();

            queryBuilder.Append("SELECT ");

            var columns = fields.FieldMappings.Values;

            queryBuilder.Append(columns
                .Select(f => "[{0}]".FormatString(f.FieldName))
                .ToDelimitedString(", "));

            queryBuilder.Append(" ");
            queryBuilder.Append("FROM {0} ".FormatString(fields.TableName));

            if (canDirtyRead)
            {
                queryBuilder.Append("(NOLOCK)");
            }

            var dbParameters = new List<IDbDataParameter>();

            if (predicate.IsNotNull())
            {
                var container = _predicateBuilder.BuildContainer(predicate, typeof(TValue), includeParameters);

                if (includeParameters)
                {
                    dbParameters = container.Parameters.ToSafeList();
                }

                queryBuilder.Append(" ");
                queryBuilder.Append("WHERE {0}".FormatString(container.WhereClause));
            }

            queryBuilder.Append(";");

            return new QueryInfo(queryBuilder.ToString().Trim(), new[] {fields}, dbParameters);
        }
    }
}
