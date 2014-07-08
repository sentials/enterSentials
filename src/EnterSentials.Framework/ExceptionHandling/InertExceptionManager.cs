using System;

namespace EnterSentials.Framework
{
    public class InertExceptionManager : ExceptionManagerBase
    {
        public override bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            exceptionToThrow = exceptionToHandle;
            return true;
        }
    }
}