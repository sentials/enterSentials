using EnterSentials.Framework.Properties;
using System;
using System.Runtime.Serialization;

namespace EnterSentials.Framework
{
    [Serializable]
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException() : this(Resources.BusinessLogicErrorMessage) { }
        public BusinessLogicException(string message) : base(message) { }
        public BusinessLogicException(string message, Exception inner) : base(message, inner) { }
        protected BusinessLogicException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
