using System.Collections.Generic;
using System.Data;

namespace TightlyCurly.Com.Common.Data.Repositories.Strategies
{
    public class DataReaderSingleBuilderStrategy : DataReaderBuilderStrategyBase, IBuilderStrategy
    {
        public DataReaderSingleBuilderStrategy(IDataReaderBuilder dataReaderBuilder) : base(dataReaderBuilder)
        {
        }

        public IEnumerable<TValue> BuildItems<TValue>(dynamic parameters, IDataReader dataSource)
            where TValue : class, new()
        {
            Guard.EnsureIsNotNull("parameters", parameters);
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var items = new List<TValue>();

            while (dataSource.Read())
            {
                items.Add(DataReaderBuilder.Build<TValue>(dataSource));
            }

            return items;
        }
    }
}
