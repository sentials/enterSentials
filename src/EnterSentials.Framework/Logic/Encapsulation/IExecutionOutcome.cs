using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IExecutionOutcome
    {
        bool Failed { get; }
        bool Completed { get; }
        bool CompletedWithoutFailure { get; }
        bool WasInvalid { get; }
        bool WasUnauthorized { get; }
        IEnumerable<Exception> Exceptions { get; }
    }


    public interface IExecutionOutcome<TResult> : IExecutionOutcome
    {
        TResult Result { get; }
    }
}