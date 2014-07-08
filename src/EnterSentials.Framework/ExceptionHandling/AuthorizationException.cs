using EnterSentials.Framework.Properties;
using System;
using System.Runtime.Serialization;
using System.Security;

namespace EnterSentials.Framework
{
    [Serializable]
    public class AuthorizationException : SecurityException
    {
        public AuthorizationException() : this(Resources.AuthorizationErrorMessage) { }
        public AuthorizationException(string message) : base(message) { }
        public AuthorizationException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
