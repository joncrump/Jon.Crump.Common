using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IUrlRoute : IModel
    {
        string RouteName { get; set; }
        string RouteUrl { get; set; }
        string HandlerPath { get; set; }
        IEnumerable<IUrlEntry> UrlEntries { get; set; }
    }
}
