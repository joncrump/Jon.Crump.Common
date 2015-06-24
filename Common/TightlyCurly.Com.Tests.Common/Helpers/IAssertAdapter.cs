using System;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public interface IAssertAdapter
    {
        void AreEqual(object expected, object actual, string message = null);
        void IsTrue(bool condition, string message = null);
        void AreEqual<TItem>(TItem expected, TItem actual, string message = null);
        void AreNotEqual<TItem>(TItem expected, TItem actual, string message = null);
        void IsFalse(bool condition, string message = null);
        void Inconclusive(string message);
        void IsNotNull(object value, string message = null);
        void IsNull(object value, string message = null);
    }
}
