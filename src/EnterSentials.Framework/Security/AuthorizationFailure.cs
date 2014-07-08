using EnterSentials.Framework.Properties;

namespace EnterSentials.Framework
{
    public class AuthorizationFailure : IAuthorizationFailure
    {
        public string ActionName { get; private set; }
        public string ErrorMessage { get; set; }

        public AuthorizationFailure()
        { ErrorMessage = Resources.AuthorizationErrorMessage; }

        public AuthorizationFailure(string actionName)
        {
            ErrorMessage = string.IsNullOrEmpty(actionName)
                ? Resources.AuthorizationErrorMessage
                : string.Format(Resources.AuthorizationErrorMessageFormat, actionName);
        }
    }
}