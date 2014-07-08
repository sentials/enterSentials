using System;

namespace EnterSentials.Framework.Services.WCF
{
    public abstract class WebService
    {
        private readonly FluidExecutableObjectExecutor fluidExecutableObjectExecutor = new FluidExecutableObjectExecutor();


        protected IUnitOfWorkFactory UnitOfWork
        { get; private set; }


        protected TOutcome Get<TOutcome>(ExecutionContext executionContext) where TOutcome : IExecutionOutcome
        { return fluidExecutableObjectExecutor.Get<TOutcome>(executionContext); }

        protected ExecutionContext ByExecuting<TExecutableObject>(UnitOfWorkSelection uowSelection) where TExecutableObject : IExecutableObject
        { return fluidExecutableObjectExecutor.ByExecuting<TExecutableObject>(uowSelection); }

        protected UnitOfWorkSelection Within(IUnitOfWork uow)
        { return fluidExecutableObjectExecutor.Within(uow); }


        public WebService(IUnitOfWorkFactory uowFactory)
        {
            Guard.AgainstNull(uowFactory, "uowFactory");
            UnitOfWork = uowFactory;
        }
    }
}