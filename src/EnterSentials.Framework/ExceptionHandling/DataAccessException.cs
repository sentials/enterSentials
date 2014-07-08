using EnterSentials.Framework.Properties;
using System;
using System.Runtime.Serialization;

namespace EnterSentials.Framework
{
    [Serializable]
    public class DataAccessException : Exception
    {
        public DataAccessException() : this(Resources.DataAccessErrorMessage) { }
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception inner) : base(message, inner) { }
        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
