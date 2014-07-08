namespace EnterSentials.Framework
{
    public class ComponentsBasedUnitOfWorkFactory : UnitOfWorkFactoryBase,  IUnitOfWorkFactory
    {
        private readonly IComponents components = null;


        protected override IUnitOfWork NewUnitOfWorkWhileLocked()
        { return components.Get<IUnitOfWork>(); }


        public ComponentsBasedUnitOfWorkFactory(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }
    }
}