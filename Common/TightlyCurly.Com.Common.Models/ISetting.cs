using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ISetting : IModel
    {
        ILocale Locale { get; set; }
        string Key { get; set; }
        string Value { get; set; }
        bool IsActive { get; set; }
        IEnumerable<ISettingSection> Sections { get; set; }
        IEnumerable<ISetting> Settings { get; set; } 
    }
}