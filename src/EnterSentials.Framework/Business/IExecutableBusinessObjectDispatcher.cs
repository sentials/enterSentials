namespace EnterSentials.Framework
{
    public interface IExecutableBusinessObjectDispatcher
    {
        void Dispatch(IExecutableObject executableBusinessObject);
    }
}