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
    [Table(Tables.IngredientComments)]
    public class IngredientCommentDataModel : ValueFactoryModelBase, IIngredientComment
    {
        private ILocale _locale;
        private IUser _user;
        private IEnumerable<IIngredientComment> _children;

        [FieldMetadata(Columns.IngredientCommentId, SqlDbType.UniqueIdentifier, Parameters.IngredientCommentId)]
        public Guid Id { get; set; }
     
        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Comment, SqlDbType.NVarChar, Parameters.Comment)]
        public string Comment { get; set; }

        public ILocale Locale
        {
            get
            {
                if (_locale.IsNull())
                {
                    _locale = GetOrLoadLazyValue(_locale, LoaderKeys.IngredientCommentLocale);
                }

                return _locale;
            }
            set { _locale = value; }
        }
        
        [FieldMetadata(Columns.ParentId, SqlDbType.UniqueIdentifier, Parameters.ParentId)]
        public Guid? ParentId { get; set; }

        public IUser User 
        { 
            get
            {
                if (_user.IsNull())
                {
                    _user = GetOrLoadLazyValue(_user, LoaderKeys.IngredientCommentUser);
                }

                return _user;
            }
            set { _user = value; }
        }

        public IEnumerable<IIngredientComment> Children
        {
            get
            {
                if (_children.IsNull())
                {
                    _children = GetOrLoadLazyValue(_children, LoaderKeys.IngredientCommentChildren);
                }

                return _children;
            }
            set { _children = value; }
        }

        [FieldMetadata(Columns.IngredientId, SqlDbType.UniqueIdentifier, Parameters.IngredientId)]
        public Guid IngredientId { get; set; }
    }
}
