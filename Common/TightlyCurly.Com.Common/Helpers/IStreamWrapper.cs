using System.IO;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface IStreamWrapper
    {
        Stream Value { get; }

        void WriteLine(string value = null);
        void Write(string value = null);
    }
}
