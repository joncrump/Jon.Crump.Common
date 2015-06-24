using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ICompany : IModel
    {
        string Name { get; set; }
        IEnumerable<ICountry> Countries { get; set; }
        IEnumerable<IContactInfo> ContactInfos { get; set; }
        Guid? ParentCompanyId { get; set; }
        ICompany ParentCompany { get; set; }
        IEnumerable<ICompany> ChildCompanies { get; set; }
        IEnumerable<ICompanyRelationshipNode> Relationships { get; set; }
        IEnumerable<ICompanyType> CompanyType { get; set; }
    }
}
