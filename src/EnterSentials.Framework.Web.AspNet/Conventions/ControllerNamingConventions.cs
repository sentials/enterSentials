using EnterSentials.Framework.Web.AspNet.Properties;
using System;

namespace EnterSentials.Framework.Web.AspNet
{
    public static class ControllerNamingConventions
    {
        private static readonly string ControllerNameSuffix = Settings.Default.ConventionalControllerNameSuffix;
        private static readonly int ControllerStringLength = ControllerNameSuffix.Length;
        private static readonly string ControllerTypeNameFormat = string.Format("{{0}}{0}", ControllerNameSuffix);
        private static readonly string ApiControllerRouteFormat = "api/{0}";


        public static string GetControllerTypeName(this string controllerName, bool shouldAddApiRoutePrefix = false)
        {
            Guard.AgainstNullOrEmpty(controllerName, "controllerName");

            controllerName = controllerName.EndsWith(ControllerNameSuffix) ?
                controllerName :
                string.Format(ControllerTypeNameFormat, controllerName);

            return shouldAddApiRoutePrefix
                ? string.Format(ApiControllerRouteFormat, controllerName)
                : controllerName;
        }


        public static string GetNameWithoutControllerSuffix(this Type type, bool shouldAddApiRoutePrefix = false)
        {
            Guard.AgainstNull(type, "type");

            var controllerName = type.Name.EndsWith(ControllerNameSuffix) ?
                type.Name.Substring(0, type.Name.Length - ControllerStringLength) :
                type.Name;

            return shouldAddApiRoutePrefix
                ? string.Format(ApiControllerRouteFormat, controllerName)
                : controllerName;
        }
    }
}
