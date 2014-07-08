using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public class ContextManagingUnitOfWork : ScopeLimitedObject, IUnitOfWork
    {
        private readonly IDictionary<Type, object> componentsByType = new Dictionary<Type, object>();

        private ICollection<object> components = new Collection<object>();
        private BufferingEventAggregator bufferingEventAggregator = null;


        protected BufferingEventAggregator BufferingEventAggregator
        { get { return bufferingEventAggregator ?? (bufferingEventAggregator = GetInfrastructureComponent<BufferingEventAggregator>()); } }


        public IUnitOfWorkFactory Factory
        { get { return GetInfrastructureComponent<IUnitOfWorkFactory>(); } }

        public IQueryFactory Queries
        { get { return GetInfrastructureComponent<IQueryFactory>(); } }

        public ICommandFactory Commands
        { get { return GetInfrastructureComponent<ICommandFactory>(); } }

        public IEventAggregator Events
        { get { return BufferingEventAggregator; } }

        public IExceptionManager ExceptionManager
        { get { return GetInfrastructureComponent<IExceptionManager>(); } }


        protected virtual TComponent GetInfrastructureComponent<TComponent>()
        {
            var component = default(TComponent);
            Guard.Against(!TryGet(out component), "Component of that type does not exist");
            return component;
        }


        private IEnumerable<object> GetUniqueComponents()
        {
            var uniqueComponents = new Collection<object>();

            foreach (var component in components)
            {
                if (!uniqueComponents.Contains(component))
                    uniqueComponents.Add(component);

                var rootContext = component;
                while (rootContext is IContextProvider)
                {
                    rootContext = ((IContextProvider)rootContext).GetContext();
                    if (!uniqueComponents.Contains(rootContext))
                        uniqueComponents.Add(rootContext);
                }
            }

            return uniqueComponents;
        }


        private void EstablishUniqueComponents()
        { components = GetUniqueComponents().ToCollection(); }


        public virtual bool TryGet(Type componentType, out object component)
        {
            component = (object)null;
            return componentsByType.TryGetValue(componentType, out component);
        }


        public bool TryGet<TComponent>(out TComponent component)
        {
            var couldGetOrNot = false;
            component = default(TComponent);

            try
            {
                var componentObject = (object)null;
                if (TryGet(typeof(TComponent), out componentObject))
                {
                    component = (TComponent)componentObject;
                    couldGetOrNot = true;
                }
            }
            catch
            { }

            return couldGetOrNot;
        }


        public object Get(Type type)
        {
            var component = (object) null;
            Guard.Against(!TryGet(type, out component), "Component of that type does not exist");
            return component;
        }

        public TComponent Get<TComponent>()
        { return (TComponent)Get(typeof(TComponent)); }


        public virtual IUnitOfWork Add(Type componentType, object component)
        {
            Guard.AgainstNull(componentType, "componentType");
            Guard.AgainstNull(component, "component");
            components.Add(componentsByType[componentType] = component);
            return this;
        }


        public IUnitOfWork Add<TComponent>(TComponent component)
        { return Add(typeof(TComponent), component); }


        public bool TryAdd(Type componentType, object component)
        {
            var couldAddOrNot = false;

            try
            {
                Add(componentType, component);
                couldAddOrNot = true;
            }
            catch
            { }

            return couldAddOrNot;
        }


        public bool TryAdd<TComponent>(TComponent component)
        { return TryAdd(typeof(TComponent), component); }


        public virtual void Commit()
        {
            foreach (var context in components)
            {
                var committable = context as ICommittable;
                if (committable != null)
                    committable.Commit();
            }

            BufferingEventAggregator.PublishAll();
        }


        protected virtual void OnDisposeExplicitAfterCommit()
        {
            foreach (var contextAndComponent in components)
            {
                var disposable = contextAndComponent as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            componentsByType.Clear();
            components.Clear();
        }


        sealed protected override void OnDisposeExplicit()
        {
            EstablishUniqueComponents();
            Commit();
            OnDisposeExplicitAfterCommit();
        }
    }
}