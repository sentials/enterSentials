using System;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    public class WebServiceProxy<TService>
    {
        protected class ServiceOperationRequestOutcome<TResponse> where TResponse : ServiceOperationResponse, new()
        {
            public bool Completed { get; private set; }
            public TResponse Response { get; private set; }

            public ServiceOperationRequestOutcome(bool completedOrNot, TResponse response = default(TResponse))
            {
                Completed = completedOrNot;
                Response = response;
            }
        }



        protected IWebServiceClientFactory WebServiceClients
        { get; private set; }

        protected string ServiceUrlBase
        { get; private set; }


        protected virtual TResponse DispatchServiceOperationRequest<TResponse>(Func<IWebServiceClient, ServiceOperationRequestOutcome<TResponse>> executeServiceOperationRequest)
             where TResponse : ServiceOperationResponse, new()
        {
            var ableToCompleteServiceOperationRequest = false;
            var exception = (Exception)null;
            var response = default(TResponse);

            using (var serviceClient = WebServiceClients.GetWebServiceClient())
            {
                try
                { 
                    var outcome = executeServiceOperationRequest(serviceClient);
                    ableToCompleteServiceOperationRequest = outcome.Completed;
                    response = outcome.Response;
                }
                catch (Exception ex)
                { exception = ex; }
            }

            if (!ableToCompleteServiceOperationRequest)
            {
                if (exception == null)
                    exception = new ServiceOperationDispatchSerializationException();

                response = Activator.CreateInstance<TResponse>();
                response.Errors = new ServiceOperationError[] { new ServiceOperationError(exception) };
                response.RefreshDerivableState();
            }

            return response;
        }


        protected TResponse Get<TResponse>(Expression<Action<TService>> serviceOperationCallingExpression) 
            where TResponse : ServiceOperationResponse, new()
        {
            return DispatchServiceOperationRequest(serviceClient =>
            {
                var response = default(TResponse);
                var completedOrNot = serviceClient.Get(
                    UrlFor(serviceOperationCallingExpression),
                    out response);
                return new ServiceOperationRequestOutcome<TResponse>(completedOrNot, response);
            });
        }


        protected TResponse Post<TResponse>(Expression<Action<TService>> serviceOperationCallingExpression, object data) 
            where TResponse : ServiceOperationResponse, new()
        {
            return DispatchServiceOperationRequest(serviceClient => 
            {
                var response = default(TResponse);
                var completedOrNot = serviceClient.Post(
                    UrlFor(serviceOperationCallingExpression),
                    data,
                    out response);
                return new ServiceOperationRequestOutcome<TResponse>(completedOrNot, response);
            });
        }


        protected virtual string UrlFor(string serviceOperationName)
        { return string.Format("{0}{1}", ServiceUrlBase, serviceOperationName); }


        protected string UrlFor(Expression<Action<TService>> serviceOperationCallingExpression)
        { return UrlFor(Name.Of.Method.On(serviceOperationCallingExpression)); }


        public WebServiceProxy(IWebServiceClientFactory webServiceClients, string serviceUrlBase)
        {
            Guard.AgainstNull(webServiceClients, "webServiceClients");
            Guard.AgainstNullOrEmpty(serviceUrlBase, "serviceUrlBase");
            WebServiceClients = webServiceClients;
            ServiceUrlBase = serviceUrlBase;
        }
    }
}
