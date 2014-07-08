using EnterSentials.Framework.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/utilizing-command-pattern-to-support.html
    public abstract class UnitOfWorkBasedExecutableObject : IExecutableObject
    {
        private readonly FluidExecutableObjectExecutor fluidExecutableObjectExecutor = new FluidExecutableObjectExecutor();


        protected IUnitOfWork CurrentUnitOfWork
        { get; private set; }

        protected IUnitOfWorkFactory UnitOfWork
        { get { return CurrentUnitOfWork.Factory; } }


        protected virtual bool ValidateParameters(out IEnumerable<ValidationResult> validationResults)
        {
            validationResults = Enumerable.Empty<ValidationResult>();
            return true; 
        }

        protected virtual bool AuthorizeForThisAction(out IEnumerable<IAuthorizationFailure> authorizationFailures)
        {
            authorizationFailures = Enumerable.Empty<IAuthorizationFailure>();
            return true;
        }


        protected void Publish<TEvent>(TEvent @event)
        { CurrentUnitOfWork.Events.Publish(@event); }


        protected abstract void ExecuteLogic();



        private bool ParametersAreValid(out IEnumerable<Exception> exceptions)
        {
            exceptions = Enumerable.Empty<Exception>();

            try 
            { 
                var validationResults = (IEnumerable<ValidationResult>)null;
                if (!ValidateParameters(out validationResults))
                {
                    var validationExceptions = (validationResults == null) 
                        ? Enumerable.Empty<ValidationException>() 
                        : validationResults.Select(vr => vr.ToException()).ToArray();

                    exceptions = validationExceptions.Any()
                        ? validationExceptions.Cast<Exception>()
                        : new ValidationException(string.Format("Validation failed for parameters of '{0}'", this.GetType().Name)).ToEnumerable();
                }
            }
            catch (Exception ex) 
            { exceptions = new ValidationException(string.Format("Validation failed for parameters of '{0}'", this.GetType().Name), ex).ToEnumerable(); }

            return !exceptions.Any();
        }


        private bool IsAuthorizedForThisAction(out IEnumerable<Exception> exceptions)
        {
            exceptions = Enumerable.Empty<Exception>();
            
            try 
            { 
                var authorizationFailures = (IEnumerable<IAuthorizationFailure>)null;
                if (!AuthorizeForThisAction(out authorizationFailures))
                {
                    var authorizationExceptions = (authorizationFailures == null)
                        ? Enumerable.Empty<AuthorizationException>()
                        : authorizationFailures.Select(af => af.ToException()).ToArray();

                    exceptions = authorizationExceptions.Any()
                        ? authorizationExceptions.Cast<Exception>()
                        : new AuthorizationException(Resources.AuthorizationErrorMessage).ToEnumerable();
                }
            }
            catch (Exception ex) 
            { exceptions = new AuthorizationException(Resources.AuthorizationErrorMessage, ex).ToEnumerable(); }

            return !exceptions.Any();
        }


        private bool AbleToCompleteExecutionOfLogic(out IEnumerable<Exception> exceptions)
        {
            exceptions = Enumerable.Empty<Exception>();

            try
            { ExecuteLogic(); }
            catch (Exception ex)
            { exceptions = (ex is AggregateException) ? ((AggregateException)ex).InnerExceptions : ex.ToEnumerable(); }

            return !exceptions.Any();
        }


        private IExecutionOutcome GetExecutionOutcome()
        {
            var wasInvalid = false;
            var wasUnauthorized = false;
            var completed = false;
            var exceptions = (IEnumerable<Exception>) null;
            

            if (ParametersAreValid(out exceptions))
            {
                if (IsAuthorizedForThisAction(out exceptions))
                {
                    if (AbleToCompleteExecutionOfLogic(out exceptions))
                        completed = true;
                }
                else
                    wasUnauthorized = true;
            }
            else 
                wasInvalid = true;

            var consolidatedException = (Exception)null;
            if ((exceptions != null) && (exceptions.TryConsolidation(out consolidatedException)))
            {
                var newException = (Exception)null;
                if (CurrentUnitOfWork.ExceptionManager.HandleException(consolidatedException, ExceptionPolicy.ShieldFromBusinessLogicExceptions, out newException))
                {
                    if (newException != null)
                        exceptions = newException.ToEnumerable();
                }
            }

            return new ExecutionOutcome(wasInvalid, wasUnauthorized, completed, exceptions);
        }


        protected virtual void AddOutcomeToUnitOfWork(IExecutionOutcome outcome)
        { CurrentUnitOfWork.Add<IExecutionOutcome>(outcome); }

        public void Execute()
        {
            var outcome = GetExecutionOutcome();
            AddOutcomeToUnitOfWork(outcome);
        }


        protected TOutcome Get<TOutcome>(ExecutionContext executionContext) where TOutcome : IExecutionOutcome
        { return fluidExecutableObjectExecutor.Get<TOutcome>(executionContext); }

        protected ExecutionContext ByExecuting<TExecutableObject>(UnitOfWorkSelection uowSelection) where TExecutableObject : IExecutableObject
        { return fluidExecutableObjectExecutor.ByExecuting<TExecutableObject>(uowSelection); }

        protected UnitOfWorkSelection Within(IUnitOfWork uow)
        { return fluidExecutableObjectExecutor.Within(uow); }


        public UnitOfWorkBasedExecutableObject(IUnitOfWork uow)
        {
            Guard.AgainstNull(uow, "uow");
            CurrentUnitOfWork = uow;
        }
    }



    public abstract class UnitOfWorkBasedExecutableObject<TResult> : UnitOfWorkBasedExecutableObject
    {
        private TResult result = default(TResult);


        public class Outcome : ExecutionOutcome<TResult>
        {
            public Outcome(TResult result, IExecutionOutcome outcome) : base(result, outcome)
            { }
        }


        protected abstract TResult ExecuteLogicWithResult();


        sealed protected override void AddOutcomeToUnitOfWork(IExecutionOutcome outcome)
        { 
            CurrentUnitOfWork.Add<Outcome>(new Outcome(result, outcome));
            CurrentUnitOfWork.Add<IExecutionOutcome>(outcome);
            if (outcome.CompletedWithoutFailure)
                CurrentUnitOfWork.Add<TResult>(result);
        }

        
        sealed protected override void ExecuteLogic()
        { result = ExecuteLogicWithResult(); }


        public UnitOfWorkBasedExecutableObject(IUnitOfWork uow) : base(uow)
        { }
    }



    public abstract class UnitOfWorkBasedExecutableObject<TResult, TParameters> : UnitOfWorkBasedExecutableObject<TResult>
    {
        protected TParameters Params
        { get; private set; }


        protected override bool ValidateParameters(out IEnumerable<ValidationResult> validationResults)
        {
            var thisValidationResults = Enumerable.Empty<ValidationResult>();
            var selfValidatingParameters = Params as IValidatable;

            var thisIsValid = (selfValidatingParameters != null)
                ? selfValidatingParameters.IsValid(out thisValidationResults)
                : Params.IsValidByDataAnnotations(out thisValidationResults);

            var baseValidationResults = (IEnumerable<ValidationResult>)null;
            var baseIsValid = base.ValidateParameters(out baseValidationResults);

            validationResults = thisValidationResults.Concat(baseValidationResults).ToArray();

            return thisIsValid && baseIsValid;
        }


        public UnitOfWorkBasedExecutableObject(TParameters parameters, IUnitOfWork uow) : base(uow)
        {
            Guard.AgainstNull(parameters, "parameters");
            Params = parameters;
        }
    }
}