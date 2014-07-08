namespace EnterSentials.Framework
{
    public class ExceptionManagingExecutableBusinessObjectDispatcher : ExecutableBusinessObjectDispatcherBase
    {
        protected IExceptionManager ExceptionManager
        { get; private set; }

        protected string ExceptionPolicy
        { get; set; }


        public override void Dispatch(IExecutableObject executableBusinessObject)
        {
            ExceptionManager.UsePolicyToProcessAction(
                ExceptionPolicy,
                () => base.Dispatch(executableBusinessObject)
            );
        }


        public ExceptionManagingExecutableBusinessObjectDispatcher(IExceptionManager exceptionManager)
        {
            Guard.AgainstNull(exceptionManager, "exceptionManager");

            this.ExceptionManager = exceptionManager;
            this.ExceptionPolicy = EnterSentials.Framework.ExceptionPolicy.ShieldFromBusinessLogicExceptions;
        }
    }
}