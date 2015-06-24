using System;
using System.Collections.Generic;
using System.Text;

namespace TightlyCurly.Com.Framework.Data
{
    public class DataObjectException : ApplicationException
    {
        // Default constructor
        public DataObjectException()
        {
        }

        // Constructor accepting a single string message
        public DataObjectException(string message)
            : base(message)
        {
        }

        // Constructor accepting a string message and an inner exception 
        // that will be wrapped by this custom exception class
        public DataObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
