using EnterSentials.Framework.Web.AspNet.Properties;
using System;
using System.Web;

namespace EnterSentials.Framework.Web.AspNet
{
    public class ExceptionManagerBasedHttpApplicationExceptionHandler : IHttpApplicationExceptionHandler
    {
        private readonly string policyName = Settings.Default.HttpApplicationGlobalExceptionPolicyName;


        protected IExceptionManager ExceptionManager
        { get; private set; }


        public void HandleExceptionOnApplication(Exception exception, HttpApplication application)
        {
            if (!string.IsNullOrEmpty(policyName))
            {
                var exceptionToThrow = (Exception)null;
                if (ExceptionManager.HandleException(exception, policyName, out exceptionToThrow))
                {
                    if (exceptionToThrow == null)
                        throw exception;
                    else
                        throw exceptionToThrow;
                }
            }
        }


        public ExceptionManagerBasedHttpApplicationExceptionHandler(IExceptionManager exceptionManager)
        {
            Guard.AgainstNull(exceptionManager, "exceptionManager");
            ExceptionManager = exceptionManager;
        }
    }
}