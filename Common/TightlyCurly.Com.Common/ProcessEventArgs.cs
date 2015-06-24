using System;

namespace TightlyCurly.Com.Common
{
    public class ProcessEventArgs : EventArgs
    {
        public ProcessEventArgs(DateTime dateOccurred, string message, EventLevel eventLevel)
        {
            DateOccurred = dateOccurred;
            Message = message;
            EventLevel = eventLevel;
        }

        public DateTime DateOccurred { get; private set; }
        public string Message { get; private set; }
        public EventLevel EventLevel { get; private set; }
    }
}
