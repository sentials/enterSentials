using System;

namespace EnterSentials.Framework
{
    public class ExceptionManagingDomainLogicDispatcher : IDomainLogicDispatcher
    {
        protected IExceptionManager ExceptionManager
        { get; private set; }

        protected string ExceptionPolicy
        { get; set; }


        protected void ProcessWithinExceptionPolicy(Action action, string exceptionPolicy)
        { ExceptionManager.UsePolicyToProcessAction(exceptionPolicy, action); }

        protected TResult ProcessedWithinExceptionPolicy<TResult>(Func<TResult> func, string exceptionPolicy)
        {
            var result = default(TResult);
            ExceptionManager.UsePolicyToProcessAction(exceptionPolicy, () => result = func());
            return result;
        }

        protected void ProcessWithinExceptionPolicy(Action action)
        { ProcessWithinExceptionPolicy(action, ExceptionPolicy); }

        protected TResult ProcessedWithinExceptionPolicy<TResult>(Func<TResult> func)
        { return ProcessedWithinExceptionPolicy(func, ExceptionPolicy); }


        public void Dispatch(Action action)
        { ProcessWithinExceptionPolicy(action, ExceptionPolicy); }

        public TResult Dispatch<TResult>(Func<TResult> func)
        { return ProcessedWithinExceptionPolicy(func, ExceptionPolicy); }


        public ExceptionManagingDomainLogicDispatcher(IExceptionManager exceptionManager)
	    {
            Guard.AgainstNull(exceptionManager, "exceptionManager");

            this.ExceptionManager = exceptionManager;
            this.ExceptionPolicy = EnterSentials.Framework.ExceptionPolicy.ShieldFromDataAccessExceptions;
	    }
    }
}
