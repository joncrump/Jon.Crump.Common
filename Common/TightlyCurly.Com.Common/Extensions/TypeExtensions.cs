using System;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class TypeExtensions
    {
        // Found this code at http://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck) 
        {
            while (toCheck != null && toCheck != typeof(object)) 
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                
                if (generic == cur) 
                {
                    return true;
                }
                
                toCheck = toCheck.BaseType;
            }
            
            return false;
        }
    }
}
