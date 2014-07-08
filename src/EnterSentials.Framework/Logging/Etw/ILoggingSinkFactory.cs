namespace EnterSentials.Framework
{
    public interface ILoggingSinkFactory
    {
        ILoggingSink Get(LoggingSinkConfiguration configuration);
    }
}
