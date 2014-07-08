namespace EnterSentials.Framework
{
    public class AttemptResponse : ServiceOperationResponse
    {
        public bool WasSuccessful { get; set; }

        public AttemptResponse()
        { }

        public AttemptResponse(IExecutionOutcome outcome) : base(outcome)
        { }
    }
}