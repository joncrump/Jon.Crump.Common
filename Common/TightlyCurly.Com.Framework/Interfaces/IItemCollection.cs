using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lmts.Framework
{
    public interface IItemCollection<T>
    {
       bool Contains(int id);
       bool Contains(T item);
    }
}
