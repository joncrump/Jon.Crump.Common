using System;
using System.Configuration;

namespace TightlyCurly.Com.Common.Configuration
{
    public class ConfigurationApplicationSettings : IApplicationSettings
    {
        public Guid ApplicationId
        {
            get
            {
                return new Guid(ConfigurationManager.AppSettings["applicationId"]);
            }
        }
    }
}
