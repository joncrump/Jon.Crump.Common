using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TightlyCurly.Com.Framework;//.EventArgs;

namespace TightlyCurly.Com.Framework.Utilities
{
    public class ObservableCollection<T> : Collection<T>
    {
        #region Fields

        private Collection<T> _removedItems = new Collection<T>();
        private Collection<T> _addedItems = new Collection<T>();
        public delegate void CollectionChangedHandler(object sender, CollectionChangedEventArgs<T> item);
        public event CollectionChangedHandler ItemAdded;
        public event CollectionChangedHandler ItemRemoved;

        #endregion

        #region Constructors

        public ObservableCollection()
            : base()
        {
        }

        public ObservableCollection(IList<T> list)
            : base(list)
        {
        }

        #endregion

        #region Properties

        public Collection<T> RemovedItems
        {
            get
            {
                return _removedItems;
            }
        }

        public Collection<T> AddedItems
        {
            get
            {
                return _addedItems;
            }
        }

        #endregion

        #region Methods

        public new void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            _addedItems.Add(item);
            base.Add(item);
            OnItemAdded(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public void AddRange(T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public new void Insert(int index, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            _addedItems.Insert(index, item);
            base.Insert(index, item);
            OnItemAdded(item);
        }

        public new void InsertItem(int index, T item)
        {
            Insert(index, item);
        }

        public new void Remove(T item)
        {
            _removedItems.Add(item);
            base.Remove(item);
            OnItemRemoved(item);
        }

        public new void RemoveAt(int index)
        {
            T item = base[index];
            _removedItems.Add(item);
            base.RemoveAt(index);
            OnItemRemoved(item);
        }

        private void OnItemAdded(T item)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, new CollectionChangedEventArgs<T>(item));
            }
        }

        private void OnItemRemoved(T item)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, new CollectionChangedEventArgs<T>(item));
            }
        }

        #endregion
    }
}
