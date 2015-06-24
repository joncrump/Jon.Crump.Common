using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Model.Enums;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.MessageTemplates)]
    public class MessageTemplateDataModel : ValueFactoryModelBase, IMessageTemplate
    {
        private ILocale _locale;
        private IEnumerable<IMessageTemplateToken> _messageTemplateTokens;
        
        [FieldMetadata(Columns.MessageTemplateId, SqlDbType.UniqueIdentifier, Parameters.MessageTemplateId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        public ILocale Locale
        {
            get
            {
                if (_locale.IsNull())
                {
                    _locale = GetOrLoadLazyValue(_locale, LoaderKeys.MessageTemplateLocale);
                }

                return _locale;
            }
            set { _locale = value; }
        }

        [FieldMetadata(Columns.Text, SqlDbType.NVarChar, Parameters.Text)]
        public string Text { get; set; }

        [FieldMetadata(Columns.ContentType, SqlDbType.Int, Parameters.UpdatedDate)]
        public ContentType? ContentType { get; set; }

        [FieldMetadata(Columns.MessageCampaignId, SqlDbType.UniqueIdentifier, Parameters.MessageCampaignId)]
        public Guid MessageCampaignId { get; set; }

        [FieldMetadata(Columns.LocaleId, SqlDbType.UniqueIdentifier, Parameters.LocaleId)]
        public Guid LocaleId { get; set; }

        [FieldMetadata(Columns.TemplateName, SqlDbType.NVarChar, Parameters.TemplateName)]
        public string TemplateName { get; set; }

        public IEnumerable<IMessageTemplateToken> MessageTemplateTokens
        {
            get
            {
                if (_messageTemplateTokens.IsNull())
                {
                    _messageTemplateTokens = GetOrLoadLazyValue(_messageTemplateTokens,
                                                                LoaderKeys.MessageTemplateMessageTemplateTokens);
                }

                return _messageTemplateTokens;
            }
            set { _messageTemplateTokens = value; }
        }
    }
}
