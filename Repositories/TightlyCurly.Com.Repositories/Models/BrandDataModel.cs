using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Brands)]
    public class BrandDataModel : ValueFactoryModelBase, IBrand
    {
        private IManufacturer _manufacturer;
        private IEnumerable<ICopyright> _copyrights;
        private IEnumerable<ITrademark> _trademarks; 

        [FieldMetadata(Columns.BrandId, SqlDbType.UniqueIdentifier, Parameters.BrandId, isPrimaryKey: true)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.BrandName, SqlDbType.NVarChar, Parameters.BrandName)]
        public string BrandName { get; set; }

        [FieldMetadata(Columns.ManufacturerId, SqlDbType.UniqueIdentifier, Parameters.ManufacturerId, allowDbNull: true)]
        public Guid? ManufacturerId { get; set; }

        [ValueFactory(LoaderKeys.GetManufacturerByBrand)]
        [Join(JoinType.Left, typeof(ManufacturerDataModel), Columns.ManufacturerId, Columns.ManufacturerId)]
        public IManufacturer Manufacturer
        {
            get
            {
                if (_manufacturer.IsNull())
                {
                    _manufacturer = GetOrLoadLazyValue(_manufacturer, LoaderKeys.GetManufacturerByBrand);
                }

                return _manufacturer;
            }
            set
            {
                _manufacturer = value;
            }
        }

        [ValueFactory(LoaderKeys.GetCopyrightsByBrand)]
        [Join(JoinType.Left, typeof(CopyrightDataModel), Columns.BrandId, Columns.CopyrightId, "dbo.Brands_Copyrights", 
            Columns.BrandId, Columns.CopyrightId, JoinType.Left)]
        public IEnumerable<ICopyright> Copyrights
        {
            get
            {
                if (_copyrights.IsNull())
                {
                    _copyrights = GetOrLoadLazyValue(_copyrights, LoaderKeys.GetCopyrightsByBrand);
                }

                return _copyrights;
            }
            set { _copyrights = value; }
        }

        [ValueFactory(LoaderKeys.GetTrademarksByBrand)]
        [Join(JoinType.Left, typeof(TrademarkDataModel), Columns.BrandId, Columns.TrademarkId, "dbo.Brands_Trademarks", 
            Columns.BrandId, Columns.TrademarkId, JoinType.Left)]
        public IEnumerable<ITrademark> Trademarks
        {
            get
            {
                if (_trademarks.IsNull())
                {
                    _trademarks = GetOrLoadLazyValue(_trademarks, LoaderKeys.GetTrademarksByBrand);
                }

                return _trademarks;
            }
            set { _trademarks = value; }
        }
    }
}
