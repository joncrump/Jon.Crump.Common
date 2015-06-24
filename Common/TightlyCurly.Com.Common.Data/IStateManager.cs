namespace TightlyCurly.Com.Common.Data
{
    public interface IStateManager
    {
        void SetState(object value);
        ObjectStateInfo GetObjectState(object value);
        void RemoveState(object value);
    }
}
