using System.Reflection;
using System.Resources;

namespace TightlyCurly.Com.Common.Helpers
{
    public class ResourcesHelper : IResourcesHelper
    {
        public TValue GetResource<TValue>(string resourceName, string key, Assembly assembly)
        {
            var resourceManager = new ResourceManager(resourceName, assembly);

            var value = typeof (TValue) == typeof (string) 
                ? resourceManager.GetString(key) 
                : resourceManager.GetObject(key);

            return (TValue) value;
        }
    }
}
