using System;
using System.Web;

namespace EnterSentials.Framework.Web.AspNet
{
    public interface IHttpApplicationExceptionHandler
    {
        void HandleExceptionOnApplication(Exception ex, HttpApplication application);
    }
}
