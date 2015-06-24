using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TightlyCurly.Com.Framework
{
    public interface IChildObject
    {
        int? ParentId { get; set; }
    }
}
