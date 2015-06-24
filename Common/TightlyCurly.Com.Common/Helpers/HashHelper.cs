namespace TightlyCurly.Com.Common.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string GenerateHash(object value)
        {
            Guard.EnsureIsNotNull("value", value);

            if (value is IHashable)
            {
                return ((IHashable) value).Hash;
            }

            return value.GetHashCode().ToString();
        }
    }
}
