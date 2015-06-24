using System;
using System.Collections.Generic;
using System.Text;

namespace TightlyCurly.Com.Framework.Interfaces
{
    public interface IParser
    {
        object Parse(Dictionary<string, object> values);
    }
}
