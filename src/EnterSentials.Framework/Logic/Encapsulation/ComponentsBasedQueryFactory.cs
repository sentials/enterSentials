using System;
namespace EnterSentials.Framework
{
    public class ComponentsBasedQueryFactory : IQueryFactory
    {
        private readonly IComponents components = null;

        public TQuery Get<TQuery>() where TQuery : IQuery
        { return components.Get<TQuery>(); }

        public IQuery Get(Type queryType)
        { return (IQuery)components.Get(queryType); }


        public ComponentsBasedQueryFactory(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }
    }
}