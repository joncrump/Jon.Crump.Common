using System;

namespace TightlyCurly.Com.Common
{
    public interface IModel 
    {
        Guid Id { get; set; }
        DateTime EnteredDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}