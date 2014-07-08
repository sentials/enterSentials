namespace EnterSentials.Framework
{
    public interface IEmailDispatcher
    {
        bool TrySend(Email email);
    }
}