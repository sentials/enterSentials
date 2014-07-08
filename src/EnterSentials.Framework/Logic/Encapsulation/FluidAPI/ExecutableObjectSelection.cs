namespace EnterSentials.Framework
{
    public class ExecutableObjectSelection<TExecutableObject> : IExecutableContext where TExecutableObject : IExecutableObject
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public IExecutableObject GetExecutableObject()
        {
            return typeof(TExecutableObject).Implements<ICommand>()
                ? UnitOfWork.Commands.Get(typeof(TExecutableObject))
                : (IExecutableObject)UnitOfWork.Queries.Get(typeof(TExecutableObject));
        }

        public ExecutableObjectSelection(UnitOfWorkSelection uowSelection)
        {
            Guard.AgainstNull(uowSelection, "uowSelection");
            UnitOfWork = uowSelection.UnitOfWork;
        }
    }
}
