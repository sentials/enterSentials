namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/utilizing-command-pattern-to-support.html
    public abstract class UnitOfWorkBasedCommand<TResult> 
        : UnitOfWorkBasedExecutableObject<TResult>, ICommand
    {
        public UnitOfWorkBasedCommand(IUnitOfWork uow) : base(uow)
        { }
    }



    public abstract class UnitOfWorkBasedCommand<TResult, TParameters> 
        : UnitOfWorkBasedExecutableObject<TResult, TParameters>, ICommand
    {
        public UnitOfWorkBasedCommand(TParameters parameters, IUnitOfWork uow) : base(parameters, uow)
        { }
    }
}