using EnterSentials.Framework;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace EnterSentials.Framework.Domain.EF
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public class DbContextBasedDomainContext : ObjectContextBasedDomainContext
    {
        private static readonly string DeleteStatementConflictedMessagePrefix = "The DELETE statement conflicted";


        private DbContext dbContext = null;


        protected override ObjectContext ObjectContext
        { get { return ((IObjectContextAdapter)dbContext).ObjectContext; } }


        protected IDomainLogicDispatcher DomainLogicDispatcher
        { get; private set; }

        protected string DefaultExceptionPolicy
        { get; set; }


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeExplicit();
            dbContext.Dispose();
            dbContext = null;
        }


        public override void Commit()
        {
            DomainLogicDispatcher.Dispatch(
                () =>
                {
                    try
                    { base.Commit(); }
                    catch (UpdateException ex)
                    {
                        if ((ex.InnerException != null) && (ex.InnerException.Message.StartsWith(DeleteStatementConflictedMessagePrefix)))
                            throw new UnableToRemoveEntityDueToRelationshipConstraintsException(ex);
                        else
                            throw;
                    }
                }
            );
        }


        public DbContextBasedDomainContext(DbContext dbContext, IDomainLogicDispatcher domainLogicDispatcher)
        {
            Guard.AgainstNull(dbContext, "dbContext");
            Guard.AgainstNull(domainLogicDispatcher, "domainLogicDispatcher");

            this.dbContext = dbContext;
            this.DomainLogicDispatcher = domainLogicDispatcher;
        }
    }
}
