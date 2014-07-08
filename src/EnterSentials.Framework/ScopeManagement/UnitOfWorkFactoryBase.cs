using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public abstract class UnitOfWorkFactoryBase : IUnitOfWorkFactory
    {
        private readonly object thisLock = new object();
        private readonly Stack<IUnitOfWork> unitsOfWork = new Stack<IUnitOfWork>();
        private IUnitOfWork current = null;


        protected IUnitOfWork GetCurrentUnitOfWorkWhileLocked()
        { return current; }

        protected void SetCurrentUnitOfWorkWhileLocked(IUnitOfWork uow)
        {  current = uow; }


        protected abstract IUnitOfWork NewUnitOfWorkWhileLocked();


        protected virtual void OnUnitOfWorkDisposed(IUnitOfWork uow)
        { }

        private void InternalOnUnitOfWorkDisposed(object sender, EventArgs e)
        {
            lock (thisLock)
            {
                var uow = Unwired((ObservableUnitOfWork)sender);

                Guard.Against(uow != unitsOfWork.Peek(), "Units of work must be disposed in order.");
                unitsOfWork.Pop();
                SetCurrentUnitOfWorkWhileLocked(unitsOfWork.Any() ? unitsOfWork.Peek() : null);
                OnUnitOfWorkDisposed(uow.EncapsulatedUnitOfWork);
            }
        }


        protected virtual void OnUnitOfWorkCreatedWhileLocked(ObservableUnitOfWork uow)
        { }

        protected virtual ObservableUnitOfWork WiredUp(ObservableUnitOfWork uow)
        {
            uow.Disposed -= InternalOnUnitOfWorkDisposed;
            uow.Disposed += InternalOnUnitOfWorkDisposed;
            return uow;
        }

        protected virtual ObservableUnitOfWork Unwired(ObservableUnitOfWork uow)
        {
            uow.Disposed -= InternalOnUnitOfWorkDisposed;
            return uow;
        }


        public IUnitOfWork Container()
        {
            lock (thisLock)
            {
                var uow = WiredUp(new ObservableUnitOfWork(NewUnitOfWorkWhileLocked()));
                SetCurrentUnitOfWorkWhileLocked(uow);
                unitsOfWork.Push(uow);
                OnUnitOfWorkCreatedWhileLocked((ObservableUnitOfWork)uow);
                return uow;
            }
        }


        protected class ObservableUnitOfWork : ScopeLimitedObject, IUnitOfWork
        {
            public IUnitOfWork EncapsulatedUnitOfWork { get; private set; }


            public IUnitOfWorkFactory Factory
            { get { return EncapsulatedUnitOfWork.Factory; } }

            public IQueryFactory Queries
            { get { return EncapsulatedUnitOfWork.Queries; } }

            public ICommandFactory Commands
            { get { return EncapsulatedUnitOfWork.Commands; } }

            public IEventAggregator Events
            { get { return EncapsulatedUnitOfWork.Events; } }

            public IExceptionManager ExceptionManager
            { get { return EncapsulatedUnitOfWork.ExceptionManager; } }


            public bool TryAdd(Type componentType, object component)
            { return EncapsulatedUnitOfWork.TryAdd(componentType, component); }

            public bool TryAdd<TComponent>(TComponent component)
            { return EncapsulatedUnitOfWork.TryAdd(component); }

            public IUnitOfWork Add(Type componentType, object component)
            { return EncapsulatedUnitOfWork.Add(componentType, component); }

            public IUnitOfWork Add<TComponent>(TComponent component)
            { return EncapsulatedUnitOfWork.Add(component); }

            public bool TryGet(Type componentType, out object component)
            { return EncapsulatedUnitOfWork.TryGet(componentType, out component); }

            public bool TryGet<TComponent>(out TComponent component)
            { return EncapsulatedUnitOfWork.TryGet(out component); }

            public object Get(Type type)
            { return EncapsulatedUnitOfWork.Get(type); }

            public TComponent Get<TComponent>()
            { return EncapsulatedUnitOfWork.Get<TComponent>(); }

            public void Commit()
            { EncapsulatedUnitOfWork.Commit(); }


            private void OnDispose()
            {
                if (EncapsulatedUnitOfWork != null)
                    EncapsulatedUnitOfWork.Dispose();
                EncapsulatedUnitOfWork = null;
            }


            protected override void OnDisposeExplicit()
            {
                base.OnDisposeExplicit();
                OnDispose();
            }


            protected override void OnDisposeImplicit()
            {
                base.OnDisposeImplicit();
                OnDispose();
            }


            public ObservableUnitOfWork(IUnitOfWork unitOfWork)
            {
                Guard.AgainstNull(unitOfWork, "unitOfWork");
                EncapsulatedUnitOfWork = unitOfWork;
            }
        }
    }
}
