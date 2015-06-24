using System.Configuration;

namespace TightlyCurly.Com.Common.Configuration
{
    public class SettingElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }
    }
}
