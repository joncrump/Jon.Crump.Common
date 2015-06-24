using System;
using TightlyCurly.Com.Common.Model.Enums;

namespace TightlyCurly.Com.Common.Models
{
    public interface ILink : IModel
    {
        LinkKind? LinkKind { get; set; }
        Uri Uri { get; set; }
    }
}
