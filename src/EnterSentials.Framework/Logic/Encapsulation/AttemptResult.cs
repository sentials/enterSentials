namespace EnterSentials.Framework
{
    public class AttemptResult
    {
        public bool WasSuccessful { get; private set; }

        public AttemptResult(bool wasSuccessful)
        { WasSuccessful = wasSuccessful; }
    }
}