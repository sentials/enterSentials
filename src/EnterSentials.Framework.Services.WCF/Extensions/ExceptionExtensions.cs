using System;
using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF
{
    public static class ExceptionExtensions
    {
        public static bool IsFaultException(this Exception exception)
        { return typeof(FaultException).IsInstanceOfType(exception); }
    }
}
