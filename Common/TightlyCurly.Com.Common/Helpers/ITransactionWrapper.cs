using System;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface ITransactionWrapper : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
