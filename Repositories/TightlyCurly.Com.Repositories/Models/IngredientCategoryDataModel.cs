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
    [Table(Tables.IngredientCategories)]
    public class IngredientCategoryDataModel : ValueFactoryModelBase, IIngredientCategory
    {
        private IEnumerable<ICategoryType> _categoryTypes;

        [FieldMetadata(Columns.IngredientCategoryId, SqlDbType.UniqueIdentifier, Parameters.IngredientCategoryId)]
        public Guid Id { get; set; }
        
        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }
        
        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }

        public IEnumerable<ICategoryType> CategoryTypes
        {
            get
            {
                if (_categoryTypes.IsNull())
                {
                    _categoryTypes = GetOrLoadLazyValue(_categoryTypes, LoaderKeys.IngredientCategoryTypes);
                }

                return _categoryTypes;
            }
            set { _categoryTypes = value; }
        }
    }
}
