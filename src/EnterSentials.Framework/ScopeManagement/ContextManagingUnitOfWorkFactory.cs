using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class ContextManagingUnitOfWorkFactory : UnitOfWorkFactoryBase, IUnitOfWorkFactory
    {
        protected override IUnitOfWork NewUnitOfWorkWhileLocked()
        { return new ContextManagingUnitOfWork(); }
    }
}