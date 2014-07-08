using System;
using System.Linq;

namespace EnterSentials.Framework
{
    internal abstract class UnitOfWorkBasedEventObserverBase<TEvent> : IObserver<TEvent>
    {
        private readonly IUnitOfWorkFactory unitOfWork = null;


        public void OnCompleted()
        { }


        public void OnError(Exception error)
        { }


        protected abstract object ResolveComponentUsing(IUnitOfWork uow);


        protected virtual void EstablishContextForUnitOfWorkWithEvent(IUnitOfWork uow, TEvent @event)
        {
            uow.Add(@event);
            if (@event is IContextProvidingEvent)
                ((IContextProvidingEvent) @event).GetContext().ForEach(tuple => uow.Add(tuple.Item1, tuple.Item2));            
        }


        public void OnNext(TEvent @event)
        {
            using (var uow = unitOfWork.Container())
            {
                EstablishContextForUnitOfWorkWithEvent(uow, @event);

                var component = ResolveComponentUsing(uow);

                if (component is IObserver<TEvent>)
                    ((IObserver<TEvent>)component).OnNext(@event);
                else
                {
                    if (component is IExecutableObject)
                    {
                        ((IExecutableObject)component).Execute();

                        var execution = uow.Get<IExecutionOutcome>();
                        if (execution.Failed)
                        {
                            Guard.Against(
                                !execution.Exceptions.Any(),
                                "Exceptions must be available if execution of a component fails during event handling.");

                            if (execution.Exceptions.Count() == 1)
                                throw execution.Exceptions.First();
                            else
                                throw new AggregateException(execution.Exceptions);
                        }
                    }
                }
            }
        }


        public UnitOfWorkBasedEventObserverBase(IUnitOfWorkFactory unitOfWorkFactory)
        {
            Guard.AgainstNull(unitOfWorkFactory, "unitOfWorkFactory");
            this.unitOfWork = unitOfWorkFactory;
        }
    }



    internal class UnitOfWorkBasedEventObserver<TEvent, TComponent> : UnitOfWorkBasedEventObserverBase<TEvent>
    {
        protected override object ResolveComponentUsing(IUnitOfWork uow)
        {
            var componentType = typeof(TComponent);
            return (componentType.Implements<ICommand>())
                ? uow.Commands.Get(componentType)
                : uow.Get(componentType);
        }


        public UnitOfWorkBasedEventObserver(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        { }
    }



    internal class UnitOfWorkBasedEventObserver<TEvent> : UnitOfWorkBasedEventObserver<TEvent, ExecutePipeline<TEvent>>
    {
        private readonly string componentKey = null;


        protected override void EstablishContextForUnitOfWorkWithEvent(IUnitOfWork uow, TEvent @event)
        {
            uow.Add(
                new ExecutePipeline<TEvent>.Parameters
                {
                    PipelineKey = componentKey,
                    Context = @event
                }
            );

            base.EstablishContextForUnitOfWorkWithEvent(uow, @event);
        }


        public UnitOfWorkBasedEventObserver(string componentKey, IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        {
            Guard.AgainstNullOrEmpty(componentKey, "componentKey");
            this.componentKey = componentKey;
        }
    }
}
