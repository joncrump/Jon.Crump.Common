using System;
using System.Collections.Generic;
using System.Text;

namespace TightlyCurly.Com.Framework.Data
{
    public class DataObjectMissingException : ApplicationException
    {
        // Default constructor
        public DataObjectMissingException()
        {
        }

        // Constructor accepting a single string message
        public DataObjectMissingException(string message)
            : base(message)
        {
        }

        // Constructor accepting a string message and an inner exception 
        // that will be wrapped by this custom exception class
        public DataObjectMissingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
