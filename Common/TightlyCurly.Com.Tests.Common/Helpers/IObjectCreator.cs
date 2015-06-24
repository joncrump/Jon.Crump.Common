using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public interface IObjectCreator
    {
        T CreateNew<T>(IDictionary<string, object> parameters = null) where T : class, new();
    }
}
