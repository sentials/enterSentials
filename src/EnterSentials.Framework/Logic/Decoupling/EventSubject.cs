using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnterSentials.Framework
{
    // Borrowed and adapted from EnterpriseLibrary 6, Semantic Logging Application Block
    internal class EventSubject<TEvent> : ScopeLimitedObject, IObservable<TEvent>, IObserver<TEvent>, IEventManager<TEvent>
    {
        private ReadOnlyCollection<IObserver<TEvent>> observers = new List<IObserver<TEvent>>().AsReadOnly();
        private readonly IComponents components = null;
        private bool isFrozen = false;


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeExplicit();
            OnCompleted();
        }


        public IDisposable Subscribe(IObserver<TEvent> observer)
        {
            Guard.AgainstNull(observer, "observer");

            if (!isFrozen)
            {
                var copy = observers.ToList();
                copy.Add(observer);
                observers = copy.AsReadOnly();
                return new Subscription(this, observer);
            }

            observer.OnCompleted();
            return new InsertDisposable();
        }


        private IDisposable Subscribe(Type componentType)
        { return Subscribe((IObserver<TEvent>)components.Get(typeof(UnitOfWorkBasedEventObserver<,>).MakeGenericType(typeof(TEvent), componentType))); }

        public IDisposable Subscribe<TComponent>()
        { return Subscribe(components.Get<UnitOfWorkBasedEventObserver<TEvent, TComponent>>()); }


        public IDisposable Subscribe(string componentKey)
        {
            return componentKey.IsAssemblyQualifiedTypeName()
                ? Subscribe(Type.GetType(componentKey))
                : Subscribe((IObserver<TEvent>)new UnitOfWorkBasedEventObserver<TEvent>(componentKey, components.Get<IUnitOfWorkFactory>()));
        }


        private void Unsubscribe(IObserver<TEvent> observer)
        { observers = observers.Where(o => !observer.Equals(o)).ToList().AsReadOnly(); }


        public void OnCompleted()
        {
            var currentObservers = TakeObserversAndFreeze();
            if (currentObservers != null)
                currentObservers.ForEach(observer => observer.OnCompleted());
        }


        public void OnError(Exception error)
        {
            var currentObservers = TakeObserversAndFreeze();
            if (currentObservers != null)
                currentObservers.ForEach(observer => observer.OnError(error));
        }


        public void OnNext(TEvent value)
        {
            foreach (var observer in observers)
                observer.OnNext(value);
        }


        private ReadOnlyCollection<IObserver<TEvent>> TakeObserversAndFreeze()
        {
            if (!isFrozen)
            {
                isFrozen = true;
                var copy = observers;
                observers = new List<IObserver<TEvent>>().AsReadOnly();
                return copy;
            }

            return null;
        }


        public EventSubject(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }


        private sealed class Subscription : ScopeLimitedObject
        {
            private IObserver<TEvent> observer = null;
            private EventSubject<TEvent> subject = null;

            protected override void OnDisposeExplicit()
            {
                base.OnDisposeExplicit();

                var observerReference = observer;
                observer = null;
                if (observerReference != null)
                {
                    subject.Unsubscribe(observerReference);
                    subject = null;
                }
            }

            public Subscription(EventSubject<TEvent> subject, IObserver<TEvent> observer)
            {
                Guard.AgainstNull(subject, "subject");
                Guard.AgainstNull(observer, "observer");
                this.subject = subject;
                this.observer = observer;
            }
        }


        private sealed class InsertDisposable : ScopeLimitedObject
        { }
    }
}