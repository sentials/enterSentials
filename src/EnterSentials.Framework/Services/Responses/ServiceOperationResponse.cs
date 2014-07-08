using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public class ServiceOperationResponse
    {
        public bool Failed { get; set; }
        public bool Completed { get; set; }
        public bool CompletedWithoutFailure { get; set; }
        public IEnumerable<ServiceOperationError> Errors { get; set; }


        protected internal void RefreshDerivableState()
        {
            Failed = !Completed || Errors.Any();
            CompletedWithoutFailure = Completed && !Failed;
        }


        public ServiceOperationResponse()
        {
            Completed = true;
            Errors = Enumerable.Empty<ServiceOperationError>();
            RefreshDerivableState();
        }


        public ServiceOperationResponse(IExecutionOutcome outcome) : this()
        {
            Guard.AgainstNull(outcome, "outcome");

            Completed = outcome.Completed;
            if (outcome.Exceptions != null)
                Errors = outcome.Exceptions.Select(ex => new ServiceOperationError(ex)).ToArray();

            RefreshDerivableState();
        }
    }
}