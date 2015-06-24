using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface IUser : IPerson
    {
        string Password { get; set; }
        DateTime? InactiveDate { get; set; }
        bool IsActive { get; set; }
        Uri Avatar { get; set; }
        

        //TODO: Put groups here.
    }
}
