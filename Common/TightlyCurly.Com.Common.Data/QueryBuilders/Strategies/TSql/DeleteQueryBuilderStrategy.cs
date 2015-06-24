using System;
using System.Linq.Expressions;
using System.Text;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class DeleteQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IPredicateBuilder _predicateBuilder;

        public DeleteQueryBuilderStrategy(IFieldHelper fieldHelper, IPredicateBuilder predicateBuilder) 
            : base(fieldHelper)
        {
            _predicateBuilder = Guard.EnsureIsNotNull("predicateBuilder", predicateBuilder);
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters) where TValue : class
        {
            Guard.EnsureIsNotNull("parameters", parameters);

            Expression<Func<TValue, bool>> predicate = parameters.Predicate;
            string tableName = parameters.TableName;

            var fields = FieldHelper.BuildFields<TValue>(tableName: tableName);

            var queryBuilder = new StringBuilder();

            queryBuilder.Append("DELETE ");
            queryBuilder.Append("FROM {0}".FormatString(fields.TableName));

            queryBuilder.Append(" ");

            var container = _predicateBuilder.BuildContainer(predicate, typeof(TValue));

            queryBuilder.Append("WHERE {0}".FormatString(container.WhereClause));

            queryBuilder.Append(";");

            return new QueryInfo(queryBuilder.ToString().Trim(), new[] {fields}, container.Parameters);
        }
    }
}
