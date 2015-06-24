using System.Collections.Generic;
using System.Linq;

namespace TightlyCurly.Com.Tests.Common
{
    public class DataContainer
    {
        public int NumberOfRecords { get; set; }
        public IList<ColumnInfo> Results { get; set; }

        public DataContainer()
        {
            Results = new List<ColumnInfo>();
        }

        public DataContainer(int numberOfRecords, IEnumerable<ColumnInfo> results)
        {
            NumberOfRecords = numberOfRecords;
            Results = results.ToList();
        }
    }
}