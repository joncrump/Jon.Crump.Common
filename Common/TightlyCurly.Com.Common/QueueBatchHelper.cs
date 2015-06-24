using System;
using System.Collections.Generic;
using System.Linq;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common
{
    public class QueueBatchHelper : EventProcessorBase, IBatchHelper
    {
        protected Queue<Tuple<int, IEnumerable<object>>> BatchQueue;

        public QueueBatchHelper()
        {
            BatchQueue = new Queue<Tuple<int, IEnumerable<object>>>();
        }

        public int SplitItemsIntoBatches<TItem>(IEnumerable<TItem> items, int numberOfItemsPerBatch)
        {
            Guard.EnsureIsNotNullOrEmpty("items", items);
            Guard.EnsureIsValid("numberOfItemsPerBatch", i => i > 0, numberOfItemsPerBatch);

            var itemCount = items.Count();
            var jobCount = 0;
            int index;
            var segment = new List<object>();

            OnProcessEvent(EventLevel.Information, "Creating batches for {0} items.".FormatString(itemCount));

            for (index = 0; index < itemCount; index++)
            {
                var item = items.ElementAt(index);

                segment.Add(item);

                OnProcessEvent(EventLevel.Information, "Added item {0} to existing segment.".FormatString(item));

                if (((index + 1) % numberOfItemsPerBatch == 0))
                {
                    BatchQueue.Enqueue(new Tuple<int, IEnumerable<object>>(++jobCount, segment));
                    segment = new List<object>();

                    OnProcessEvent(EventLevel.Information, "Creating new segment");
                }
            }

            if (segment.IsNotNullOrEmpty())
            {
                BatchQueue.Enqueue(new Tuple<int, IEnumerable<object>>(++jobCount, segment));
            }

            return BatchQueue.Count();
        }

        public IEnumerable<TItem> GetBatch<TItem>(int batchId)
        {
            Guard.EnsureIsValid("batchId", i => i > 0, batchId);

            var batch = BatchQueue.FirstOrDefault(t => t.Item1 == batchId);

            if (batch.IsNull())
            {
                throw new InvalidOperationException("Could not retrieve batch id {0}.  Batch id does not exist or has already been processed."
                    .FormatString(batchId.ToString()));
            }

            var items = batch.Item2;

            return items.IsNullOrEmpty() 
                ? Enumerable.Empty<TItem>() : items.Select(s => (TItem) s);
        }

        public bool HasBatches()
        {
            return BatchQueue.Any();
        }

        public IEnumerable<TItem> GetNextBatch<TItem>()
        {
            if (!BatchQueue.Any())
            {
                throw new InvalidOperationException("Cannot execute batches.  No batches to process.");
            }

            var item = BatchQueue.Dequeue();

            return item.IsNull() 
                ? Enumerable.Empty<TItem>() 
                : item.Item2.Select(i => (TItem) i);
        }
    }
}