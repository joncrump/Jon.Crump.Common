using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface ISitemapEntry : IModel
    {
        Uri Uri { get; set; }
        string ChangeFrequency { get; set; }
        string Priority { get; set; }
    }
}
