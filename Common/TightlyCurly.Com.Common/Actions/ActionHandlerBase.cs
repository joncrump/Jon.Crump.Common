using System;

namespace TightlyCurly.Com.Common.Actions
{
    public abstract class ActionHandlerBase
    {
        public event ProcessEventHandler ProcessEventOccurred;

        protected virtual void OnProcessEvent(ProcessEventArgs eventArgs)
        {
            if (ProcessEventOccurred != null)
            {
                ProcessEventOccurred(this, eventArgs);
            }
        }

        protected void OnProcessEvent(EventLevel eventLevel, string message)
        {
            if (ProcessEventOccurred != null)
            {
                ProcessEventOccurred(this, new ProcessEventArgs(DateTime.Now, message, eventLevel));
            }
        }

        protected void Info(string message)
        {
            OnProcessEvent(EventLevel.Information, message);
        }

        protected void Error(string message)
        {
            OnProcessEvent(EventLevel.Error, message);
        }

        protected void Warning(string message)
        {
            OnProcessEvent(EventLevel.Warning, message);
        }
    }
}
