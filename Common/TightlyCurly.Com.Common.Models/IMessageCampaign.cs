using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMessageCampaign : IModel
    {
        string CampaignName { get; set; }
        string Description { get; set; }
        IEnumerable<IMessageTemplate> MessageTemplates { get; set; }
        ILocale Locale { get; set; } 
        IEnumerable<ISubscription> Subscriptions { get; set; }
        Guid MessageTemplateId { get; set; }
        Guid LocaleId { get; set; }
    }
}
