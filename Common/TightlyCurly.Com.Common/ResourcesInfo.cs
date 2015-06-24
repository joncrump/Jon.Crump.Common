using System.Reflection;

namespace TightlyCurly.Com.Common
{
    public class ResourcesInfo : IResourcesInfo
    {
        public string ResourceName { get; set; }
        public string Key { get; set; }
        public Assembly Assembly { get; set; }
    }
}
