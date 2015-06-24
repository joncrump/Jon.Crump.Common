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
    [Table(Tables.ContactInfos)]
    public class ContactInfoDataModel : ValueFactoryModelBase, IContactInfo
    {
        private IEnumerable<IPerson> _people;
        private IEnumerable<IAddress> _addresses;
        private IEnumerable<IPhoneNumber> _phoneNumbers;
        private IEnumerable<IEmailAddress> _emailAddresses;
        private IEnumerable<ISocialMediaInfo> _socialMedia;
        private IEnumerable<ICompanyPosition> _companyPositions;
        private IEnumerable<IPerson> _contacts;

        [FieldMetadata(Columns.ContactInfoId, SqlDbType.UniqueIdentifier, Parameters.ContactInfoId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }
        
        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.Notes, SqlDbType.NVarChar, Parameters.Notes, allowDbNull: true)]
        public string Notes { get; set; }
        
        [FieldMetadata(Columns.Description, SqlDbType.NVarChar, Parameters.Description)]
        public string Description { get; set; }

        [FieldMetadata(Columns.IsActive, SqlDbType.Bit, Parameters.IsActive, allowDbNull: true)]
        public bool? IsActive { get; set; }

        [FieldMetadata(Columns.Title, SqlDbType.NVarChar, Parameters.Title, allowDbNull: true)]
        public string Title { get; set; }
        
        [ValueFactory(LoaderKeys.GetPeopleByContactInfo)]
        public IEnumerable<IPerson> People
        {
            get
            {
                if (_people.IsNull())
                {
                    _people = GetOrLoadLazyValue(_people, LoaderKeys.GetPeopleByContactInfo);
                }

                return _people;
            }
            set { _people = value; }
        }

        [ValueFactory(LoaderKeys.GetAddressesByContactInfo)]
        public IEnumerable<IAddress> Addresses
        {
            get
            {
                if (_addresses.IsNull())
                {
                    _addresses = GetOrLoadLazyValue(_addresses, LoaderKeys.GetAddressesByContactInfo);
                }

                return _addresses;
            }
            set { _addresses = value; }
        }
        
        [ValueFactory(LoaderKeys.GetPhoneNumbersByContactInfo)]
        public IEnumerable<IPhoneNumber> PhoneNumbers
        {
            get
            {
                if (_phoneNumbers.IsNull())
                {
                    _phoneNumbers = GetOrLoadLazyValue(_phoneNumbers, LoaderKeys.GetPhoneNumbersByContactInfo);
                }

                return _phoneNumbers;
            }
            set { _phoneNumbers = value; }
        }
        
        [ValueFactory(LoaderKeys.GetEmailsByContactInfo)]
        public IEnumerable<IEmailAddress> EmailAddresses
        {
            get
            {
                if (_emailAddresses.IsNull())
                {
                    _emailAddresses = GetOrLoadLazyValue(_emailAddresses, LoaderKeys.GetEmailsByContactInfo);
                }

                return _emailAddresses;
            }
            set { _emailAddresses = value; }
        }
        
        [ValueFactory(LoaderKeys.GetSocialMediaByContactInfo)]
        public IEnumerable<ISocialMediaInfo> SocialMedia
        {
            get
            {
                if (_socialMedia.IsNull())
                {
                    _socialMedia = GetOrLoadLazyValue(_socialMedia, LoaderKeys.GetSocialMediaByContactInfo);
                }

                return _socialMedia;
            }
            set { _socialMedia = value; }
        }
        
        [ValueFactory(LoaderKeys.GetCompanyPositionsByContactInfo)]
        public IEnumerable<ICompanyPosition> CompanyPositions
        {
            get
            {
                if (_companyPositions.IsNull())
                {
                    _companyPositions = GetOrLoadLazyValue(_companyPositions,
                                                           LoaderKeys.GetCompanyPositionsByContactInfo);
                }

                return _companyPositions;
            }
            set { _companyPositions = value; }
        }

        [ValueFactory(LoaderKeys.GetContactsByContactInfo)]
        public IEnumerable<IPerson> Contacts
        {
            get
            {
                if (_contacts.IsNull())
                {
                    _contacts = GetOrLoadLazyValue(_contacts, LoaderKeys.GetContactsByContactInfo);
                }

                return _contacts;
            }
            set { _contacts = value; }
        }
    }
}
