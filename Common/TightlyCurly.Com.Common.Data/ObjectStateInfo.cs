using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Data
{
    public class ObjectStateInfo
    {
        public ObjectStateInfo()
        {
            ChildInfos = new List<ObjectStateInfo>();
        }

        public Type ObjectType { get; set; }
        public string PropertyName { get; set; }
        public ObjectState ObjectState { get; set; }
        public IList<ObjectStateInfo> ChildInfos { get; set; }
        public ChildState ChildState { get; set; }
    }
}
