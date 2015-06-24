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
    [Table(Tables.IngredientRatings)]
    public class IngredientRatingDataModel : ValueFactoryModelBase, IIngredientRating
    {
        private IRatingType _ratingType;
        private IEnumerable<IIngredientRatingText> _ingredientRatingTexts;

        [FieldMetadata(Columns.IngredientRatingId, SqlDbType.UniqueIdentifier, Parameters.IngredientRatingId)]
        public Guid Id { get; set; }
        
        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
       
        public IRatingType RatingType
        {
            get
            {
                if (_ratingType.IsNull())
                {
                    _ratingType = GetOrLoadLazyValue(_ratingType, LoaderKeys.IngredientRatingRatingType);
                }

                return _ratingType;
            }
            set { _ratingType = value; }
        }

        [FieldMetadata(Columns.IngredientId, SqlDbType.UniqueIdentifier, Parameters.IngredientId)]
        public Guid IngredientId { get; set; }

        public IEnumerable<IIngredientRatingText> Texts
        {
            get
            {
                if (_ingredientRatingTexts.IsNull())
                {
                    _ingredientRatingTexts = GetOrLoadLazyValue(_ingredientRatingTexts,
                                                                LoaderKeys.IngredientRatingIngredientRatingTexts);
                }

                return _ingredientRatingTexts;
            }
            set { _ingredientRatingTexts = value; }
        }
    }
}
