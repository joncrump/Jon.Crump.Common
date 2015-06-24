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
    [Table(Tables.MessageCampaigns)]
    public class MessageCampaignDataModel : ValueFactoryModelBase, IMessageCampaign
    {
        private IEnumerable<IMessageTemplate> _messageTemplates;
        private ILocale _locale;
        private IEnumerable<ISubscription> _subscriptions;

        [FieldMetadata(Columns.MessageCampaignId, SqlDbType.UniqueIdentifier, Parameters.MessageCampaignId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.CampaignName, SqlDbType.NVarChar, Parameters.CampaignName)]
        public string CampaignName { get; set; }

        [FieldMetadata(Columns.Description, SqlDbType.NVarChar, Parameters.Description)]
        public string Description { get; set; }

        public IEnumerable<IMessageTemplate> MessageTemplates
        {
            get
            {
                if (_messageTemplates.IsNull())
                {
                    _messageTemplates = GetOrLoadLazyValue(_messageTemplates, LoaderKeys.MessageCampaignMessageTemplate);
                }

                return _messageTemplates;
            }
            set { _messageTemplates = value; }
        }
        
        public ILocale Locale
        {
            get
            {
                if (_locale.IsNull())
                {
                    _locale = GetOrLoadLazyValue(_locale, LoaderKeys.MessageCampaignLocale);
                }

                return _locale;
            }
            set { _locale = value; }
        }

        public IEnumerable<ISubscription> Subscriptions
        {
            get
            {
                if (_subscriptions.IsNull())
                {
                    _subscriptions = GetOrLoadLazyValue(_subscriptions, LoaderKeys.MessageCampaignSubscriptions);
                }

                return _subscriptions;
            }
            set { _subscriptions = value; }
        }

        [FieldMetadata(Columns.MessageTemplateId, SqlDbType.UniqueIdentifier, Parameters.MessageTemplateId)]
        public Guid MessageTemplateId { get; set; }

        [FieldMetadata(Columns.LocaleId, SqlDbType.UniqueIdentifier, Parameters.LocaleId)]
        public Guid LocaleId { get; set; }
    }
}
