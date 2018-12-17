using System;
using System.Collections.Generic;
using System.Text;

namespace net.phraustbyte.dal
{
    /// <summary>
    /// Represents an exception generated when a record is not found
    /// </summary>
    public class RecordNotFoundException:Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RecordNotFoundException(){ }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="message">Message that describes the error</param>
        public RecordNotFoundException(string message) :base(message) { }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="message">Message that describes the error</param>
        /// <param name="inner">The exception that is the cause of the current exception</param>
        public RecordNotFoundException(string message, Exception inner) :base(message,inner) { }
        
    }
}
