using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.ContentArticleNotes)]
    public class ContentArticleNoteDataModel : ValueFactoryModelBase, IContentArticleNote
    {
        private IUser _user;

        [FieldMetadata(Columns.ContentArticleNoteId, SqlDbType.UniqueIdentifier, Parameters.ContentArticleNoteId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Note, SqlDbType.NVarChar, Parameters.Note)]
        public string Note { get; set; }

        [ValueFactory(LoaderKeys.GetEnteredByByContentArticleNote)]
        public IUser EnteredBy
        {
            get
            {
                if (_user.IsNull())
                {
                    _user = GetOrLoadLazyValue(_user, LoaderKeys.User);
                }

                return _user;
            }
            set { _user = value; }
        }
    }
}
