using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface ICompanyPosition : IModel
    {
        Guid? CompanyId { get; set; }
        IPerson Person { get; set; }
        ICompany Company { get; set; }
        string Title { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        bool? IsActive { get; set; }
        string Description { get; set; }
        string Notes { get; set; }
        string Department { get; set; }
        PositionCategory? PositionCategory { get; set; }
    }
}
