using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Data
{
    public interface IStateStore
    {
        IDictionary<string, InitialObjectState> GetStateStore();
        void SaveStateStore(IDictionary<string, InitialObjectState> stateStore);
    }
}
