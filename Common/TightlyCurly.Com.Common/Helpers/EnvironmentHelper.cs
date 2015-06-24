using System;

namespace TightlyCurly.Com.Common.Helpers
{
    public class EnvironmentHelper : IEnvironmentHelper
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        public string GetUsername()
        {
            return Environment.UserName;
        }
    }
}
