using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public static class ApplicationClaim
    {
        private const char PermissionDelimiter = '|';
        private static readonly string PermissionDelimiterString = new string(new char[] { PermissionDelimiter });

        public static Claim From(string resourceName, string actionName)
        {
            Guard.AgainstNullOrEmpty(resourceName, "resourceName");
            Guard.AgainstNullOrEmpty(actionName, "actionName");
            return new Claim(ApplicationClaimTypes.Permission, string.Join(PermissionDelimiterString, resourceName, actionName));
        }

        public static bool TryExtract(this Claim claim, out string resourceName, out string actionName)
        {
            resourceName = null;
            actionName = null;

            var canOrNot = false; ;

            if (claim.Type == ApplicationClaimTypes.Permission)
            {
                var components = claim.Value.Split(PermissionDelimiter);
                if (components.Count() == 2)
                {
                    resourceName = components.ElementAt(0);
                    actionName = components.ElementAt(1);
                }
                else
                    canOrNot = false;
            }

            return canOrNot;
        }
    }
}