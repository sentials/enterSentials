using System;
using EntLibExceptionManager = Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionManager;

namespace EnterSentials.Framework.EntLib
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2011/05/building-framework-for-cross-cutting.html
    public class EntLibBasedExceptionManager : ExceptionManagerBase
    {
        private readonly EntLibExceptionManager exceptionManager = null;


        public override bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        { return exceptionManager.HandleException(exceptionToHandle, policyName, out exceptionToThrow); }

        public override bool HandleException(Exception exceptionToHandle, string policyName)
        { return exceptionManager.HandleException(exceptionToHandle, policyName); }


        public EntLibBasedExceptionManager(EntLibExceptionManager exceptionManager)
        {
            Guard.AgainstNull(exceptionManager, "exceptionManager");
            this.exceptionManager = exceptionManager;
        }
    }
}
