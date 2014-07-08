using System;

namespace EnterSentials.Framework
{
    public interface IDomainLogicDispatcher
    {
        void Dispatch(Action action);
        TResult Dispatch<TResult>(Func<TResult> func);
    }
}
