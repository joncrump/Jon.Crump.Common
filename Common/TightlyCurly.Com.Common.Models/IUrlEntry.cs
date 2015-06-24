using System;
using TightlyCurly.Com.Common.Model.Enums;

namespace TightlyCurly.Com.Common.Models
{
    public interface IUrlEntry : IModel
    {
        string Key { get; set; }
        Uri Uri { get; set; }
        string Priority { get; set; }
        ChangeFrequency ChangeFrequency { get; set; }
    }
}
