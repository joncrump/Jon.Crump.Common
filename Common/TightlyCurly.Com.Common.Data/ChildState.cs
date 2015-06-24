using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TightlyCurly.Com.Common.Data
{
    public enum ChildState : short
    {
        Unknown = 0,

        // Not found in ObjectStateManager as a child or parent.
        NotTracked = 1,
        // Found in ObjectStateManager as a child and also as a parent.
        Attached = 2,

        // Found as a parent in ObjectStateManager, but not as a child.
        Detached,

        // Found in ObjectStateManager as a child, but not as a parent.
        AttachedToParentAsChildNoParentExists
    }
}
