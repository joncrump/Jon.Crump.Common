namespace TightlyCurly.Com.Common.Models
{
    public interface IStateProvince : IModel
    {
        string Name { get; set; }
        string Abbreviation { get; set; }
        ICountry Country { get; set; }
    }
}
