using System.Collections.Generic;
using System.Dynamic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IContactInfo : IModel
    {
        string Notes { get; set; }
        string Description { get; set; }
        bool? IsActive { get; set; }
        string Title { get; set; }

        IEnumerable<IPerson> Contacts { get; set; }
        IEnumerable<IAddress> Addresses { get; set; }
        IEnumerable<IPhoneNumber> PhoneNumbers { get; set; }
        IEnumerable<IEmailAddress> EmailAddresses { get; set; }
        IEnumerable<ISocialMediaInfo> SocialMedia { get; set; }
        IEnumerable<ICompanyPosition> CompanyPositions { get; set; }
    }
}
