using System;

namespace EnterSentials.Framework
{
    public class ExecutionContext
    {
        private readonly IUnitOfWork uow = null;
        private readonly Func<IExecutableObject> getExecutableObject = null;

        public void ExecuteExecutableObject()
        { getExecutableObject().Execute(); }


        public TOutcome GetOutcome<TOutcome>() where TOutcome : IExecutionOutcome
        { return uow.Get<TOutcome>(); }


        internal ExecutionContext(IExecutableContext executableContext)
        {
            Guard.AgainstNull(executableContext, "executableContext");
            uow = executableContext.UnitOfWork;
            getExecutableObject = executableContext.GetExecutableObject;
        }

        public ExecutionContext Using<TParameters>(TParameters parameters)
        { uow.Add(parameters); return this; }
    }
}
