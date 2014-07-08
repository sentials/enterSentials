namespace EnterSentials.Framework
{
    public class FluidExecutableObjectExecutor
    {
        public TOutcome Get<TOutcome>(ExecutionContext executionContext) where TOutcome : IExecutionOutcome
        {
            executionContext.ExecuteExecutableObject();
            return executionContext.GetOutcome<TOutcome>();
        }

        public ExecutionContext ByExecuting<TExecutableObject>(UnitOfWorkSelection uowSelection) where TExecutableObject : IExecutableObject
        { return new ExecutionContext(new ExecutableObjectSelection<TExecutableObject>(uowSelection)); }

        public UnitOfWorkSelection Within(IUnitOfWork uow)
        { return new UnitOfWorkSelection(uow); }
    }
}
