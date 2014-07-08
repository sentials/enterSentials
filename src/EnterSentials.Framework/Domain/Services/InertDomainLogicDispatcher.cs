using System;

namespace EnterSentials.Framework
{
    public class InertDomainLogicDispatcher : IDomainLogicDispatcher
    {
        public void Dispatch(Action action)
        { action(); }

        public TResult Dispatch<TResult>(Func<TResult> func)
        { return func(); }
    }
}
