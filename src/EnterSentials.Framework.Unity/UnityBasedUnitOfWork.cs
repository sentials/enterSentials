using Microsoft.Practices.Unity;
using System;

namespace EnterSentials.Framework.Unity
{
    public class UnityBasedUnitOfWork : ContextManagingUnitOfWork
    {
        public IUnityContainer Container { get; private set; }


        public override IUnitOfWork Add(Type componentType, object component)
        {
            // Using container controlled lifetime so that child containers/unitsOfWork can use the component
            if (!(component  is IDomain))
                Container.RegisterInstance(componentType, component, new ContainerControlledLifetimeManager());
            return base.Add(componentType, component);
        }


        protected override TComponent GetInfrastructureComponent<TComponent>()
        { return Container.Resolve<TComponent>(); }


        protected override void OnDisposeExplicitAfterCommit()
        {
            base.OnDisposeExplicitAfterCommit();
            this.Container.Dispose();
            this.Container = null;
        }


        public UnityBasedUnitOfWork(IUnityContainer container)
        {
            Guard.AgainstNull(container, "container");
            this.Container = container;
        }
    }
}
