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
    [Table(Tables.Ingredients)]
    public class IngredientDataModel : ValueFactoryModelBase, IIngredient
    {
        private IEnumerable<IIngredientCategory> _ingredientCategories;
        private IEnumerable<string> _aliases;
        private IEnumerable<IIngredientRating> _ingredientRatings;
        private IEnumerable<string> _notes;
        private IEnumerable<IIngredientComment> _ingredientComments;
        private IEnumerable<IContentArticle> _contentArticles; 

        [FieldMetadata(Columns.IngredientId, SqlDbType.UniqueIdentifier, Parameters.IngredientId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        public IEnumerable<IIngredientCategory> IngredientCategories
        {
            get
            {
                if (_ingredientCategories.IsNull())
                {
                    _ingredientCategories = GetOrLoadLazyValue(_ingredientCategories, LoaderKeys.IngredientCategories);
                }

                return _ingredientCategories;
            }
            set { _ingredientCategories = value; }
        }
        
        [FieldMetadata(Columns.Title, SqlDbType.NVarChar, Parameters.Title)]
        public string Title { get; set; }

        public IEnumerable<string> Aliases
        {
            get
            {
                if (_aliases.IsNull())
                {
                    _aliases = GetOrLoadLazyValue(_aliases, LoaderKeys.Aliases);
                }

                return _aliases;
            }
            set { _aliases = value; }
        }

        [FieldMetadata(Columns.Description, SqlDbType.NVarChar, Parameters.Description)]
        public string Description { get; set; }

        public IEnumerable<Uri> Links { get; set; }

        public IEnumerable<IIngredientRating> IngredientRatings
        {
            get
            {
                if (_ingredientRatings.IsNull())
                {
                    _ingredientRatings = GetOrLoadLazyValue(_ingredientRatings, LoaderKeys.IngredientRatings);
                }

                return _ingredientRatings;
            }
            set { _ingredientRatings = value; }
        }
        
        public IEnumerable<string> Notes
        {
            get
            {
                if (_notes.IsNull())
                {
                    _notes = GetOrLoadLazyValue(_notes, LoaderKeys.IngredientNotes);
                }

                return _notes;
            }
            set { _notes = value; }
        }

        public IEnumerable<IIngredientComment> Comments
        {
            get
            {
                if (_ingredientComments.IsNull())
                {
                    _ingredientComments = GetOrLoadLazyValue(_ingredientComments, LoaderKeys.IngredientComments);
                }

                return _ingredientComments;
            }
            set { _ingredientComments = value; }
        }

        public IEnumerable<IContentArticle> ContentArticles
        {
            get
            {
                if (_contentArticles.IsNull())
                {
                    _contentArticles = GetOrLoadLazyValue(_contentArticles, LoaderKeys.IngredientContentArticles);
                }

                return _contentArticles;
            }
            set { _contentArticles = value; }
        }
    }
}
