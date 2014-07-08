using Microsoft.Practices.Unity;

namespace EnterSentials.Framework.Unity
{
    public class UnityBasedUnitOfWorkFactory : UnitOfWorkFactoryBase, IUnitOfWorkFactory
    {
        private readonly IUnityContainer container = null;


        protected override IUnitOfWork NewUnitOfWorkWhileLocked()
        {
            var current = GetCurrentUnitOfWorkWhileLocked();
            var uowContainer = current == null
                ?  container
                : ((UnityBasedUnitOfWork)((ObservableUnitOfWork) current).EncapsulatedUnitOfWork).Container;

            var childContainer = uowContainer.CreateChildContainer();
            return childContainer.Resolve<UnityBasedUnitOfWork>();
        }


        protected override void OnUnitOfWorkCreatedWhileLocked(ObservableUnitOfWork uow)
        {
            base.OnUnitOfWorkCreatedWhileLocked(uow);
            var childContainer = ((UnityBasedUnitOfWork)uow.EncapsulatedUnitOfWork).Container;
            childContainer.RegisterInstance<IUnitOfWork>(uow);
        }


        public UnityBasedUnitOfWorkFactory(IUnityContainer container)
        {
            Guard.AgainstNull(container, "container");
            this.container = container;
        }
    }
}
