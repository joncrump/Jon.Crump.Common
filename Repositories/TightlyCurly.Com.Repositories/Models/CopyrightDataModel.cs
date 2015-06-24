using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Copyrights)]
    public class CopyrightDataModel : ValueFactoryModelBase, ICopyright
    {
        private IEnumerable<ICompany> _owners;
        private IEnumerable<ICountry> _countries; 

        [FieldMetadata(Columns.CopyrightId, SqlDbType.UniqueIdentifier, Parameters.CopyrightId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Copyright, SqlDbType.NVarChar, Parameters.Copyright)]
        public string Copyright { get; set; }

        [FieldMetadata(Columns.IsActive, SqlDbType.Bit, Parameters.IsActive, allowDbNull: true)]
        public bool? IsActive { get; set; }

        [FieldMetadata(Columns.RegisteredDate, SqlDbType.SmallDateTime, Parameters.RegisteredDate, allowDbNull: true)]
        public DateTime? RegisteredDate { get; set; }

        [FieldMetadata(Columns.ExpirationDate, SqlDbType.SmallDateTime, Parameters.ExpirationDate, allowDbNull: true)]
        public DateTime? ExpirationDate { get; set; }

        [ValueFactory(LoaderKeys.GetOwnersByCopyright)]
        public IEnumerable<ICompany> Owners
        {
            get
            {
                if (_owners.IsNull())
                {
                    _owners = GetOrLoadLazyValue(_owners, LoaderKeys.GetOwnersByCopyright);
                }

                return _owners;
            }
            set { _owners = value; }
        }

        [ValueFactory(LoaderKeys.GetCountriesByCopyright)]
        public IEnumerable<ICountry> Countries
        {
            get
            {
                if (_countries.IsNull())
                {
                    _countries = GetOrLoadLazyValue(_countries, LoaderKeys.GetCountriesByCopyright);
                }

                return _countries;
            }
            set { _countries = value; }
        }
    }
}
