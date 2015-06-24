using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IQuestion : IModel
    {
        string Value { get; set; }
        string Answer { get; set; }
        ILocale Locale { get; set; }
        IEnumerable<IQuestionCategory> QuestionCategories { get; set; }
    }
}