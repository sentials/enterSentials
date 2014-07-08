using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace EnterSentials.Framework.Web.AspNet
{
    public class HandleExceptionsUsingPolicyAttribute : ExceptionFilterAttribute
    {
        public static readonly string DefaultPolicyName = ExceptionPolicy.ShieldFromBusinessLogicExceptions;


        private readonly IExceptionManager exceptionManager = Components.Instance.Get<IExceptionManager>();


        public string PolicyName
        { get; set; }


        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exceptionToThrow = (Exception)null;

            if (exceptionManager.HandleException(actionExecutedContext.Exception, PolicyName, out exceptionToThrow))
                actionExecutedContext.Exception = exceptionToThrow ?? actionExecutedContext.Exception;

            actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }


        public HandleExceptionsUsingPolicyAttribute()
        { PolicyName = DefaultPolicyName; }
    }
}
