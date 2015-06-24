using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IPerson : IModel
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string Gender { get; set; }
        string Honorific { get; set; }
        string Suffix { get; set; }
        ILocale PreferredLocale { get; set; }
        IEnumerable<ISocialMediaInfo> SocialMediaInfo { get; set; }

        IEnumerable<IContactInfo> ContactInfo { get; set; }
        IEnumerable<IPersonRelationship> Relationships { get; set; } 
    }
}
