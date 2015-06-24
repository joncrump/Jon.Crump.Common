using System.Collections.Generic;
using TightlyCurly.Com.Common.Actions;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface IBatchHelper
    {
        event ProcessEventHandler ProcessEventOccurred;
        int SplitItemsIntoBatches<TItem>(IEnumerable<TItem> items, int numberOfItemsPerBatch);
        IEnumerable<TItem> GetBatch<TItem>(int batchId);
        bool HasBatches();
        IEnumerable<TItem> GetNextBatch<TItem>();
    }
}
