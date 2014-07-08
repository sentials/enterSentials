namespace EnterSentials.Framework
{
    public class UnitOfWorkSelection
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkSelection(IUnitOfWork uow)
        {
            Guard.AgainstNull(uow, "uow");
            UnitOfWork = uow;
        }
    }
}