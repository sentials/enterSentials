namespace EnterSentials.Framework
{
    public abstract class ExecutableBusinessObjectDispatcherBase : IExecutableBusinessObjectDispatcher
    {
        public virtual void Dispatch(IExecutableObject executableBusinessObject)
        { executableBusinessObject.Execute(); }
    }
}
