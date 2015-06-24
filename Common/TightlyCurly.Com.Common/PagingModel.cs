using System.Collections.Generic;

namespace TightlyCurly.Com.Common
{
    public class PagingModel
    {
        public PagingModel()
        {
            Items = new List<object>();
        }

        public int CurrentPage { get; set; }
        public int RecordCount { get; set; }
        public int NumberOfPages { get; set; }
        public int RowsPerPage { get; set; }
        public IList<object> Items { get; set; } 
    }
}
