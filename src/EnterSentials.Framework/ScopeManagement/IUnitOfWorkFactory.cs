using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Container();
    }
}
