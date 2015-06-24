using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMenuItem : ILink
    {
        IEnumerable<IMenuItemText> MenuItemTexts { get; set; }
        int? Order { get; set; }
        IEnumerable<IMenuItem> Children { get; set; }
        Guid MenuId { get; set; }
    }
}