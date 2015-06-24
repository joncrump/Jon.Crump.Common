using System;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories.Models
{

    public class StateProvinceDataModel : IStateProvince
    {
        public Guid Id { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public ICountry Country { get; set; }
    }
}
