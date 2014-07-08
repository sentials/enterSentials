using System;

namespace EnterSentials.Framework
{
    public interface IQueryFactory
    {
        TQuery Get<TQuery>() where TQuery : IQuery;
        IQuery Get(Type queryType);
    }
}
