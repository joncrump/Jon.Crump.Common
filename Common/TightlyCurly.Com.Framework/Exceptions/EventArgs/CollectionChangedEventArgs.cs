using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TightlyCurly.Com.Framework
{
    public class CollectionChangedEventArgs<T> : EventArgs
    {
        #region Fields

        private T _item;

        #endregion

        #region Constructors

        public CollectionChangedEventArgs(T item)
        {
            _item = item;
        }

        #endregion

        #region Properties

        public T Item
        {
            get
            {
                return _item;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
