using System;
using System.Linq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class CountQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        public CountQueryBuilderStrategy(IFieldHelper fieldHelper) : base(fieldHelper)
        {
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters) where TValue : class
        {
            var type = typeof(TValue);

            var tableAttribute = type.GetCustomAttributes(typeof(TableAttribute), true)
                .FirstOrDefault() as TableAttribute;

            if (tableAttribute.IsNull())
            {
                throw new InvalidOperationException("Could not build query. Type {0} does not have a table attribute."
                    .FormatString(typeof(TValue)));
            }

            var found = false;
            CountMetadataAttribute countAttribute = null;

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
                throw new InvalidOperationException("Could not build query. Type {0} does not have a count attribute."
                    .FormatString(typeof(TValue).ToString()));
            }

            return new QueryInfo("SELECT COUNT({0}) AS {1} FROM {2};".FormatString(countAttribute.FieldName, countAttribute.FieldAlias,
                tableAttribute.Name));
        }
    }
}
