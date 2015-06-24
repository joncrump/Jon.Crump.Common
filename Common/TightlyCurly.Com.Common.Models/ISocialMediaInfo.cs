using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ISocialMediaInfo : IModel
    {
        ISocialMediaType SocialMediaType { get; set; }
        IDictionary<string, string> Data { get; set; } 
    }
}
