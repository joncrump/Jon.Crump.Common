using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.ContentArticleRevisions)]
    public class ContentArticleRevisionDataModel : ValueFactoryModelBase, IContentArticleRevision
    {
        private IContentArticle _contentArticle;

        [FieldMetadata(Columns.ContentArticleRevisionId, SqlDbType.UniqueIdentifier, Parameters.ContentArticleRevisionId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.VersionNumber, SqlDbType.NVarChar, Parameters.VersionNumber)]
        public string VersionNumber { get; set; }

        public IContentArticle Article
        {
            get
            {
                if (_contentArticle.IsNull())
                {
                    _contentArticle = GetOrLoadLazyValue(_contentArticle, LoaderKeys.ContentArticle);
                }

                return _contentArticle;
            }
            set { _contentArticle = value; }
        }
    }
}
