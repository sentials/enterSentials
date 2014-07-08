namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/utilizing-command-pattern-to-support.html
    public abstract class UnitOfWorkBasedQuery<TResult> 
        : UnitOfWorkBasedExecutableObject<TResult>, IQuery
    {
        protected bool ShouldExecuteAsReadOnly
        { get; private set; }


        public UnitOfWorkBasedQuery(IUnitOfWork uow, bool shouldExecuteAsReadOnly = true) : base(uow)
        { ShouldExecuteAsReadOnly = shouldExecuteAsReadOnly; }
    }


    public abstract class UnitOfWorkBasedQuery<TResult, TParameters> 
        : UnitOfWorkBasedExecutableObject<TResult, TParameters>, IQuery
    {
        protected bool ShouldExecuteAsReadOnly
        { get; private set; }


        public UnitOfWorkBasedQuery(TParameters parameters, IUnitOfWork uow, bool shouldExecuteAsReadOnly = true) : base(parameters, uow)
        { ShouldExecuteAsReadOnly = shouldExecuteAsReadOnly; }
    }
}