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
    [Table(Tables.ContentArticles)]
    public class ContentArticleDataModel : ValueFactoryModelBase, IContentArticle
    {
        private IEnumerable<IContentArticleNote> _notes;
        private IEnumerable<IContentArticleRevision> _revisions;
        private IContentItem _contentItem;
        private IEnumerable<string> _metaKeywords;
        private IEnumerable<IContentArticleData> _data;
        private ILocale _locale;

        [FieldMetadata(Columns.MetaDescription, SqlDbType.NVarChar, Parameters.MetaDescription)]
        public string MetaDescription { get; set; }

        [FieldMetadata(Columns.ContentArticleId, SqlDbType.UniqueIdentifier, Parameters.ContentArticleId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.IsActive, SqlDbType.Bit, Parameters.IsActive)]
        public bool IsActive { get; set; }

        [FieldMetadata(Columns.ContentItemId, SqlDbType.UniqueIdentifier, Parameters.ContentItemId)]
        public Guid ContentItemId { get; set; }

        [FieldMetadata(Columns.Description, SqlDbType.NVarChar, Parameters.Description, allowDbNull:true)]
        public string Description { get; set; }

        [FieldMetadata(Columns.Text, SqlDbType.Text, Parameters.Text, allowDbNull: true)]
        public string Text { get; set; }

        [FieldMetadata(Columns.LocaleId, SqlDbType.UniqueIdentifier, Parameters.LocaleId)]
        public Guid LocaleId { get; set; }

        [ValueFactory(LoaderKeys.GetNotesByContentArticle)]
        public IEnumerable<IContentArticleNote> Notes
        {
            get
            {
                if (_notes.IsNull())
                {
                    _notes = GetOrLoadLazyValue(_notes, LoaderKeys.GetNotesByContentArticle);
                }

                return _notes;
            }
            set { _notes = value; }
        }

        [ValueFactory(LoaderKeys.GetRevisionsByContentArticle)]
        public IEnumerable<IContentArticleRevision> Revisions
        {
            get
            {
                if (_revisions.IsNull())
                {
                    _revisions = GetOrLoadLazyValue(_revisions, LoaderKeys.GetRevisionsByContentArticleData);
                }

                return _revisions;
            }
            set { _revisions = value; }
        }
        
        [ValueFactory(LoaderKeys.GetContentItemByContentArticle)]
        public IContentItem ContentItem
        {
            get
            {
                if (_contentItem.IsNull())
                {
                    _contentItem = GetOrLoadLazyValue(_contentItem, LoaderKeys.GetContentItemByContentArticleData);
                }

                return _contentItem;
            }
            set { _contentItem = value; }
        }
        
        [ValueFactory(LoaderKeys.GetMetaKeywordsByContentArticle)]
        public IEnumerable<string> MetaKeywords
        {
            get
            {
                if (_metaKeywords.IsNull())
                {
                    _metaKeywords = GetOrLoadLazyValue(_metaKeywords, LoaderKeys.GetMetaKeywordsByContentArticleData);
                }

                return _metaKeywords;
            }
            set { _metaKeywords = value; }
        }
        
        [ValueFactory(LoaderKeys.GetDataByContentArticle)]
        public IEnumerable<IContentArticleData> Data
        {
            get
            {
                if (_data.IsNull())
                {
                    _data = GetOrLoadLazyValue(_data, LoaderKeys.GetDataByContentArticleData);
                }

                return _data;
            }
            set { _data = value; }
        }

        [ValueFactory(LoaderKeys.GetLocaleByContentArticle)]
        public ILocale Locale
        {
            get
            {
                if (_locale.IsNull())
                {
                    _locale = GetOrLoadLazyValue(_locale, LoaderKeys.GetLocaleByContentArticleData);
                }

                return _locale;
            }
            set { _locale = value; }
        }
    }
}