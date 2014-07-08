using EnterSentials.Framework;
using System;
using System.Web;

namespace EnterSentials.Framework.Web.AspNet
{
    public abstract class BootstrapperBasedHttpApplication : HttpApplication
    {
        protected abstract IBootstrapper NewBootstrapper();

        protected virtual IHttpApplicationExceptionHandler GetExceptionHandler()
        { return Components.Instance.Get<IHttpApplicationExceptionHandler>(); }

        protected void Application_Start()
        { NewBootstrapper().Initialize(); }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exceptionHandler = GetExceptionHandler();
            if (exceptionHandler != null)
                exceptionHandler.HandleExceptionOnApplication(Server.GetLastError(), (HttpApplication)sender);
        }
    }
}