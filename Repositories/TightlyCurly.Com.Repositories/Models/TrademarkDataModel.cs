using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories.Models
{
    public class TrademarkDataModel : ITrademark
    {
        public Guid Id { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Trademark { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public IEnumerable<ICompany> Companies { get; set; }
        public IEnumerable<ICountry> Countries { get; set; }
    }
}
