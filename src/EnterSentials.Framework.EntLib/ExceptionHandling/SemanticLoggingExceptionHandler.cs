using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace EnterSentials.Framework.EntLib
{
    // Borrowed or adapted from Enterprise Library reference implementation
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class SemanticLoggingExceptionHandler : IExceptionHandler
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "attributes")]
        public SemanticLoggingExceptionHandler(NameValueCollection attributes)
        { }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            Components.Instance.Get<ILog>().ExceptionByPolicy(exception.ToString(), handlingInstanceId);
            return exception;
        }
    }
}