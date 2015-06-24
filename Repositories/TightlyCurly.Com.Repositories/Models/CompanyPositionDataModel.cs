using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.CompanyPositions)]
    public class CompanyPositionDataModel : ValueFactoryModelBase, ICompanyPosition
    {
        private ICompany _company;
        private IPerson _person;

        [FieldMetadata(Columns.CompanyPositionId, SqlDbType.UniqueIdentifier, Parameters.CompanyPositionId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.CompanyId, SqlDbType.UniqueIdentifier, Parameters.CompanyId)]
        public Guid? CompanyId { get; set; }

        [FieldMetadata(Columns.Title, SqlDbType.NVarChar, Parameters.Title)]
        public string Title { get; set; }

        [FieldMetadata(Columns.StartDate, SqlDbType.SmallDateTime, Parameters.StartDate)]
        public DateTime? StartDate { get; set; }

        [FieldMetadata(Columns.EndDate, SqlDbType.SmallDateTime, Parameters.EndDate)]
        public DateTime? EndDate { get; set; }

        [FieldMetadata(Columns.IsActive, SqlDbType.Bit, Parameters.IsActive)]
        public bool? IsActive { get; set; }

        [FieldMetadata(Columns.Description, SqlDbType.NVarChar, Parameters.Description)]
        public string Description { get; set; }

        [FieldMetadata(Columns.Notes, SqlDbType.NVarChar, Parameters.Notes)]
        public string Notes { get; set; }

        [FieldMetadata(Columns.Department, SqlDbType.NVarChar, Parameters.Department)]
        public string Department { get; set; }

        [FieldMetadata(Columns.PositionCategory, SqlDbType.SmallInt, Parameters.PositionCategory)]
        public PositionCategory? PositionCategory { get; set; }

        [ValueFactory(LoaderKeys.GetCompanyByCompanyPosition)]
        public ICompany Company
        {
            get
            {
                if (_company.IsNull())
                {
                    _company = GetOrLoadLazyValue(_company, LoaderKeys.GetCompanyByCompanyPosition);
                }

                return _company;
            }
            set { _company = value; }
        }

        [ValueFactory(LoaderKeys.GetPersonByCompanyPosition)]
        public IPerson Person
        {
            get
            {
                if (_person.IsNull())
                {
                    _person = GetOrLoadLazyValue(_person, LoaderKeys.GetPersonByCompanyPosition);
                }

                return _person;
            }
            set { _person = value; }
        }
    }
}
