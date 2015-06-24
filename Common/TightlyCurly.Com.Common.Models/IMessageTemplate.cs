using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMessageTemplate : IModel, ITextEntry
    {
        Guid MessageCampaignId { get; set; }
        Guid LocaleId { get; set; }
        string TemplateName { get; set; }
        IEnumerable<IMessageTemplateToken> MessageTemplateTokens { get; set; }
    }
}
