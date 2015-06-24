using System.Configuration;

namespace TightlyCurly.Com.Common.Configuration
{
    public class NameTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string ProcessorType
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }
    }
}
