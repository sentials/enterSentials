using EnterSentials.Framework.Properties;
using System;

namespace EnterSentials.Framework
{
    [Serializable]
    public class UnableToRemoveEntityDueToRelationshipConstraintsException : Exception
    {
        public UnableToRemoveEntityDueToRelationshipConstraintsException(string message, Exception exception) : base(message, exception)
        { }

        public UnableToRemoveEntityDueToRelationshipConstraintsException(Exception exception) : this(Resources.CannotRemoveEntityDueToRelationshipConstraintsMessage, exception)
        { }

        public UnableToRemoveEntityDueToRelationshipConstraintsException(string message) : base(message)
        { }

        public UnableToRemoveEntityDueToRelationshipConstraintsException() : base()
        { }
    }
}