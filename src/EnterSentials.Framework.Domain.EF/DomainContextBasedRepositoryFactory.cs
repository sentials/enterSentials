namespace EnterSentials.Framework.Domain.EF
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public class DomainContextBasedRepositoryFactory : IRepositoryFactory
    {
        private readonly IDomainContext domainContext = null;
        private readonly IDomainLogicDispatcher domainLogicDispatcher = null;


        public IRepository<T> Get<T>() where T : class
        { return new DomainContextBasedRepository<T>(domainContext, domainLogicDispatcher); }

        public IReadOnlyRepository<T> GetReadOnly<T>() where T : class
        { return new ReadOnlyRepositoryWrapper<T>(Get<T>()); }


        public DomainContextBasedRepositoryFactory(IDomainContext domainContext, IDomainLogicDispatcher domainLogicDispatcher)
        {
            Guard.AgainstNull(domainContext, "domainContext");
            Guard.AgainstNull(domainLogicDispatcher, "domainLogicDispatcher");
            this.domainContext = domainContext;
            this.domainLogicDispatcher = domainLogicDispatcher;
        }
    }
}
