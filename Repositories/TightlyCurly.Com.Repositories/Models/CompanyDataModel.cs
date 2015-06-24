using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Companies)]
    public class CompanyDataModel : ValueFactoryModelBase, ICompany
    {
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IContactInfo> _contactInfos;
        private ICompany _parentCompany;
        private IEnumerable<ICompany> _childCompanies;
        private IEnumerable<ICompanyRelationshipNode> _relationships;
        private IEnumerable<ICompanyType> _companyTypes;
            
        [FieldMetadata(Columns.CompanyId, SqlDbType.UniqueIdentifier, Parameters.CompanyId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.ParentCompanyId, SqlDbType.UniqueIdentifier, Parameters.ParentCompanyId)]
        public Guid? ParentCompanyId { get; set; }

        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }

        [ValueFactory(LoaderKeys.GetCountriesByCompany)]
        public IEnumerable<ICountry> Countries
        {
            get
            {
                if (_countries.IsNull())
                {
                    _countries = GetOrLoadLazyValue(_countries, LoaderKeys.CountriesByCompany);
                }

                return _countries;
            }
            set { _countries = value; }
        }

        [ValueFactory(LoaderKeys.GetContactInfosByCompany)]
        public IEnumerable<IContactInfo> ContactInfos
        {
            get
            {
                if (_contactInfos.IsNull())
                {
                    _contactInfos = GetOrLoadLazyValue(_contactInfos, LoaderKeys.GetContactInfosByCompany);
                }

                return _contactInfos;
            }
            set { _contactInfos = value; }
        }

        [ValueFactory(LoaderKeys.GetParentCompanyByCompany)]
        public ICompany ParentCompany
        {
            get
            {
                if (_parentCompany.IsNull())
                {
                    _parentCompany = GetOrLoadLazyValue(_parentCompany, LoaderKeys.GetParentCompanyByCompany);
                }

                return _parentCompany;
            }
            set { _parentCompany = value; }
        }

        [ValueFactory(LoaderKeys.GetChildCompaniesByCompany)]
        public IEnumerable<ICompany> ChildCompanies
        {
            get
            {
                if (_childCompanies.IsNull())
                {
                    _childCompanies = GetOrLoadLazyValue(_childCompanies, LoaderKeys.GetChildCompaniesByCompany);
                }

                return _childCompanies;
            }
            set { _childCompanies = value; }
        }
        
        [ValueFactory(LoaderKeys.GetRelationshipsByCompany)]
        public IEnumerable<ICompanyRelationshipNode> Relationships
        {
            get
            {
                if (_relationships.IsNull())
                {
                    _relationships = GetOrLoadLazyValue(_relationships, LoaderKeys.GetRelationshipsByCompany);
                }

                return _relationships;
            }
            set { _relationships = value; }
        }

        [ValueFactory(LoaderKeys.GetCompanyTypesByCompany)]
        public IEnumerable<ICompanyType> CompanyType
        {
            get
            {
                if (_companyTypes.IsNull())
                {
                    _companyTypes = GetOrLoadLazyValue(_companyTypes, LoaderKeys.GetCompanyTypesByCompany);
                }

                return _companyTypes;
            }
            set { _companyTypes = value; }
        }
    }
}
