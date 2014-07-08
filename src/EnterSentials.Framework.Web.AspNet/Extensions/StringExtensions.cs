using System.Web;

namespace EnterSentials.Framework.Web.AspNet
{
    public static class StringExtensions
    {
        public static string ToAbsoluteUrlString(this string relativeUrlString)
        { return VirtualPathUtility.ToAbsolute(relativeUrlString); }
    }
}
