using System.Reflection;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface IResourcesHelper
    {
        TValue GetResource<TValue>(string resourceName, string key, Assembly assembly);
    }
}
