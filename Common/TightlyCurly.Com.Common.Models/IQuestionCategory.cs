using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IQuestionCategory : IModel
    {
        Guid? ParentId { get; set; }
        string Text { get; set; }
        ILocale Locale { get; set; }
        IEnumerable<IQuestionCategory> Children { get; set; } 
    }
}
