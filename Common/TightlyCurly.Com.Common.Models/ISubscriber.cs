using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ISubscriber : IPerson
    {
        Guid PersonId { get; set; }
        IUser Client { get; set; }
        string EmailAddress { get; set; }
        MessageFormat PreferredFormat { get; set; }
        ILocale PreferredLocale { get; set; }
        IEnumerable<ISubscription> Subscriptions { get; set; }
        bool IsConfirmed { get; set; }
        DateTime? ConfirmedDate { get; set; }
    }
}
