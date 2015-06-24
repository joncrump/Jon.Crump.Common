using System;
using TightlyCurly.Com.Framework.Enums;

namespace TightlyCurly.Com.Framework
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RestrictionLevelAttribute : Attribute
    {
        public RestrictionLevelAttribute(RestrictionLevel restrictionLevel)
        {
            RestrictionLevel = restrictionLevel;
        }

        public RestrictionLevel RestrictionLevel { get; private set; }
    }
}
