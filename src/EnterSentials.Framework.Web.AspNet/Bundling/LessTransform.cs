using EnterSentials.Framework;
using System;
using System.Web.Optimization;

namespace EnterSentials.Framework.Web.AspNet
{
    public class LessTransform : IBundleTransform
    {
        private readonly Func<string, string> parseLess = null;

        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = parseLess(response.Content);
            response.ContentType = "text/css";
        }


        public LessTransform(Func<string, string> parseLess)
        {
            Guard.AgainstNull(parseLess, "parseLess");
            this.parseLess = parseLess;
        }
    }
}
