using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class UpdateQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IPredicateBuilder _predicateBuilder;

        public UpdateQueryBuilderStrategy(IFieldHelper fieldHelper, IPredicateBuilder predicateBuilder) 
            : base(fieldHelper)
        {
            _predicateBuilder = Guard.EnsureIsNotNull("predicateBuilder", predicateBuilder);
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters = null) where TValue : class
        {
            TValue model = parameters.Model;
            IEnumerable<string> desiredFields = parameters.DesiredFields;
            Expression<Func<TValue, bool>> predicate = parameters.Predicate;
            string tableName = parameters.TableName;

            var fields = FieldHelper.BuildFields(desiredFields, model: model, ignoreIdentity: false, tableName: tableName);

            var queryBuilder = new StringBuilder();

            queryBuilder.Append("UPDATE {0} SET ".FormatString(fields.TableName));

            var columns = fields.FieldMappings.Values;

            queryBuilder.Append(columns
                .Where(f => !f.IsIdentity)
                .Select(f => "[{0}] = {1}".FormatString(f.FieldName, f.ParameterName))
                .ToDelimitedString(", "));

            var dbParameters = FieldHelper.ExtractParameters(fields, false);

            queryBuilder.Append(" ");
            var container = _predicateBuilder.BuildContainer(predicate, typeof(TValue));
            queryBuilder.Append("WHERE {0}".FormatString(container.WhereClause));
            queryBuilder.Append(";");

            return new QueryInfo(queryBuilder.ToString().Trim(), fields, dbParameters);
        }
    }
}
