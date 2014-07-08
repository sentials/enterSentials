using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EnterSentials.Framework.Web.AspNet
{
    public static class HttpRequestMessageExtensions
    {
        public static bool IsAjaxRequest(this HttpRequestMessage request)
        {
            var xRequestedWithHeaders = (IEnumerable<string>) null;
            if (request.Headers.TryGetValues("X-Requested-With", out xRequestedWithHeaders))
            {
                var headerValue = xRequestedWithHeaders.FirstOrDefault();
                if (!String.IsNullOrEmpty(headerValue))
                    return String.Equals(headerValue, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public static bool IsRequestWithXXSRFToken(this HttpRequestMessage request)
        {
            var xRequestedWithHeaders = (IEnumerable<string>)null;
            if (request.Headers.TryGetValues("X-XSRF-Token", out xRequestedWithHeaders))
            {
                return xRequestedWithHeaders.Any();
            }

            return false;
        }
    }
}
