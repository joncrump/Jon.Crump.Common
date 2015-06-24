using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Data
{
    public class InitialObjectState
    {
        public InitialObjectState()
        {
            ChildObjectStates = new List<InitialObjectState>();
        }

        public string Id { get; set; }
        public string PropertyName { get; set; }
        public Type ObjectType { get; set; }
        public bool IsDeleted { get; set; }
        public string HashCode { get; set; }
        public IList<InitialObjectState> ChildObjectStates { get; set; }
        public string ParentId { get; set; }
    }
}
