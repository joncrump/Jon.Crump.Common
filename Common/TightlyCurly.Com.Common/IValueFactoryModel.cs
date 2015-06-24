using System;
using System.Collections;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common
{
    public interface IValueFactoryModel
    {
        IDictionary<string, Func<object>> ValueFactories { get; set; }
    }
}
