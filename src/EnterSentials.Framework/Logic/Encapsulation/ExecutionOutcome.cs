using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnterSentials.Framework
{
    public class ExecutionOutcome : IExecutionOutcome
    {
        public bool WasInvalid
        { get; protected set; }

        public bool WasUnauthorized
        { get; protected set; }

        public bool Failed
        { get { return WasInvalid || WasUnauthorized || !Completed || Exceptions.Any(); } }

        public bool Completed
        { get; protected set; }

        public bool CompletedWithoutFailure
        { get { return Completed && !Failed; } }
        

        public IEnumerable<Exception> Exceptions
        { get; protected set; }


        public ExecutionOutcome(bool wasInvalid = false, bool wasUnauthorized = false, bool completed = true, IEnumerable<Exception> exceptions = null)
        {
            WasInvalid = wasInvalid;
            WasUnauthorized = wasUnauthorized;
            Completed = !wasInvalid && !wasUnauthorized && completed;
            Exceptions = (exceptions == null) ? Enumerable.Empty<Exception>() : exceptions;
        }
    }


    public class ExecutionOutcome<TResult> : ExecutionOutcome, IExecutionOutcome<TResult>
    {
        public TResult Result
        { get; private set; }

        public ExecutionOutcome(TResult result)
        { Result = result; }

        public ExecutionOutcome(TResult result, IExecutionOutcome executionOutcome)
        { 
            Result = result;
            WasInvalid = executionOutcome.WasInvalid;
            WasUnauthorized = executionOutcome.WasInvalid;
            Completed = executionOutcome.Completed;
            Exceptions = executionOutcome.Exceptions;
        }
    }
}