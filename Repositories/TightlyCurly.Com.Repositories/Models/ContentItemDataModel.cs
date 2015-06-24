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
    [Table(Tables.ContentItems)]
    public class ContentItemDataModel : ValueFactoryModelBase, IContentItem
    {
        private IEnumerable<string> _metaKeywords;
        private IEnumerable<IContentArticle> _contentArticles;

        [FieldMetadata(Columns.ContentItemId, SqlDbType.UniqueIdentifier, Parameters.ContentItemId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.MetaDescription, SqlDbType.NVarChar, Parameters.MetaDescription)]
        public string MetaDescription { get; set; }

        public IEnumerable<string> MetaKeywords
        {
            get
            {
                if (_metaKeywords.IsNull())
                {
                    _metaKeywords = GetOrLoadLazyValue(_metaKeywords, LoaderKeys.MetaKeywords);
                }

                return _metaKeywords;
            }
            set { _metaKeywords = value; }
        }
    
        [FieldMetadata(Columns.Title, SqlDbType.NVarChar, Parameters.Title)]
        public string Title { get; set; }
        
        [FieldMetadata(Columns.Description, SqlDbType.NVarChar, Parameters.Description)]
        public string Description { get; set; }

        [FieldMetadata(Columns.IsActive, SqlDbType.BigInt, Parameters.IsActive)]
        public bool IsActive { get; set; }

        [FieldMetadata(Columns.Uri, SqlDbType.NVarChar, Parameters.Uri)]
        public Uri Uri { get; set; }

        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }

        public IEnumerable<IContentArticle> Articles
        {
            get
            {
                if (_contentArticles.IsNull())
                {
                    _contentArticles = GetOrLoadLazyValue(_contentArticles, LoaderKeys.ContentArticles);
                }

                return _contentArticles;
            }
            set { _contentArticles = value; }
        }
    }
}
