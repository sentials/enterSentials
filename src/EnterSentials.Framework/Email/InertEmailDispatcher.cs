namespace EnterSentials.Framework
{
    public class InertEmailDispatcher : IEmailDispatcher
    {
        public bool TrySend(Email email)
        { return true; }
    }
}