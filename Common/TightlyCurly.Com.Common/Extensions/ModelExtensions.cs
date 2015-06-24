using System.Collections;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class ModelExtensions
    {
        public static bool IsNew(this IModel model)
        {
            if (model == null)
            {
                return false;
            }

            return Comparer.Default.Compare(model.Id, default(object)) == 0;
        }
    }
}
