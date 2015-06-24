using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ISettingSection : IModel
    {
        Guid? ParentId { get; set; }
        ILocale Locale { get; set; } 
        IEnumerable<ISettingSection> Children { get; set; }
        IEnumerable<ISetting> Settings { get; set; } 
    }
}
