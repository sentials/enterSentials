using System.Data.Entity;

namespace EnterSentials.Framework.Domain.EF
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", 
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification="Disposable functionality has been hoisted to a governing component over this one."
    )]
    public class DbContextBasedDomain<TDbContext> : IDomain, IContextProvider where TDbContext : DbContext
    {
        private readonly IDomainContext domainContext = null;


        public IRepositoryFactory Repositories
        { get; private set; }


        public object GetContext()
        { return domainContext; }


        public DbContextBasedDomain(TDbContext domainContext, IDomainLogicDispatcher dispatcher)
        {
            Guard.AgainstNull(domainContext, "domainContext");
            Guard.AgainstNull(dispatcher, "dispatcher");

            this.domainContext = new DbContextBasedDomainContext(domainContext, dispatcher);
            Repositories = new DomainContextBasedRepositoryFactory(this.domainContext, dispatcher);
        }
    }
}
