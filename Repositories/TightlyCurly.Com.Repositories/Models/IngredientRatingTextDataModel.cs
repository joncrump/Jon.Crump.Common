using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.IngredientRatingTexts)]
    public class IngredientRatingTextDataModel : ValueFactoryModelBase, IIngredientRatingText
    {
        private ILocale _locale;

        [FieldMetadata(Columns.IngredientRatingTextId, SqlDbType.UniqueIdentifier, Parameters.IngredientRatingTextId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.Text, SqlDbType.NVarChar, Parameters.Text)]
        public string Text { get; set; }

        [FieldMetadata(Columns.LocaleId, SqlDbType.UniqueIdentifier, Parameters.LocaleId)]
        public Guid LocaleId { get; set; }
        
        public ILocale Locale
        {
            get
            {
                if (_locale.IsNull())
                {
                    _locale = GetOrLoadLazyValue(_locale, LoaderKeys.IngredientRatingTextLocale);
                }

                return _locale;
            }
            set { _locale = value; }
        }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
    }
}
