using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace EnterSentials.Framework.Web.AspNet
{
    public class ValidateHttpAntiForgeryTokenAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// This should ALWAYS be true except for testing environments where automation is being used
        /// that prevents the validation token from working!
        /// </summary>
        public static bool ValidationEnabled = true;

        private static bool Initialized = false;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //we only try this once - whether it succeeds or fails (bad bool string, etc), we don't want to 
            //keep trying it
            if (!Initialized)
            {
                Initialized = true;
                var configVal = System.Configuration.ConfigurationManager.AppSettings["AntiforgeryValidationEnabled"];
                if (!string.IsNullOrWhiteSpace(configVal))
                {
                    var enabled = true;
                    if (Boolean.TryParse(configVal, out enabled))
                        ValidationEnabled = enabled;
                }
            }
            //do our validation if we're using it
            if (ValidationEnabled)
            {
                var request = actionContext.ControllerContext.Request;

                try
                {
                    if (request.IsRequestWithXXSRFToken())
                        ValidateRequestHeaderViaXXSRFToken(request);
                    else if (request.IsAjaxRequest())
                        ValidateRequestHeader(request);
                    else
                        AntiForgery.Validate();
                }
                catch (HttpAntiForgeryException e)
                {
                    actionContext.Response = request.CreateErrorResponse(HttpStatusCode.Forbidden, e);
                }
            }
        }

        private void ValidateRequestHeaderViaXXSRFToken(HttpRequestMessage request)
        {
            var headers = request.Headers;
            var cookie = headers
                .GetCookies()
                .Select(c => c[AntiForgeryConfig.CookieName])
                .FirstOrDefault();

            IEnumerable<string> xXsrfHeaders;

            if (headers.TryGetValues("X-XSRF-Token", out xXsrfHeaders))
            {
                var rvt = xXsrfHeaders.FirstOrDefault();

                if (cookie == null)
                {
                    throw new InvalidOperationException(String.Format("Missing {0} cookie", AntiForgeryConfig.CookieName));
                }

                AntiForgery.Validate(cookie.Value, rvt);
            }
            else
            {
                var headerBuilder = new StringBuilder();

                headerBuilder.AppendLine("Missing X-XSRF-Token HTTP header:");

                foreach (var header in headers)
                {
                    headerBuilder.AppendFormat("- [{0}] = {1}", header.Key, header.Value);
                    headerBuilder.AppendLine();
                }

                throw new InvalidOperationException(headerBuilder.ToString());
            }
        }


        private void ValidateRequestHeader(HttpRequestMessage request)
        {
            string cookieToken = String.Empty;
            string formToken = String.Empty;

            IEnumerable<string> tokenHeaders;
            if (request.Headers.TryGetValues("RequestVerificationToken", out tokenHeaders))
            {
                string tokenValue = tokenHeaders.FirstOrDefault();
                if (!String.IsNullOrEmpty(tokenValue))
                {
                    string[] tokens = tokenValue.Split(':');
                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }
            }

            AntiForgery.Validate(cookieToken, formToken);
        }
    }
}
