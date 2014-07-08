namespace EnterSentials.Framework
{
    public interface IAuthorizationFailure
    {
        string ActionName
        { get; }

        string ErrorMessage
        { get; }
    }
}
