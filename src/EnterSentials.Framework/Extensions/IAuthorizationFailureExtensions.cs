namespace EnterSentials.Framework
{
    public static class IAuthorizationFailureExtensions
    {
        public static AuthorizationException ToException(this IAuthorizationFailure authorizationFailure)
        { return new AuthorizationException(authorizationFailure.ErrorMessage); }
    }
}
