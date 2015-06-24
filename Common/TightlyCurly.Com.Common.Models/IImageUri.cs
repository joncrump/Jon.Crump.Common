using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface IImageUri : IModel
    {
        Uri Uri { get; set; }
        string Description { get; set; }
        string AltText { get; set; }
        bool? IsPrimary { get; set; }
        int? Order { get; set; }
        bool? IsActive { get; set; }
    }
}
