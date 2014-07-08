namespace EnterSentials.Framework
{
    public class ComponentsBasedDomainFactory : IDomainFactory
    {
        private readonly IComponents components = null;


        public TDomain Get<TDomain>() where TDomain : IDomain
        { return components.Get<TDomain>(); }


        public ComponentsBasedDomainFactory(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }
    }
}
