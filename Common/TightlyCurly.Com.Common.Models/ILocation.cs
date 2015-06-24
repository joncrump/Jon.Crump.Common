namespace TightlyCurly.Com.Common.Models
{
    public interface ILocation : IModel
    {
        string Name { get; set; }
        IAddress Address { get; set; }
        ICoordinates Coordinates { get; set; }
    }
}
