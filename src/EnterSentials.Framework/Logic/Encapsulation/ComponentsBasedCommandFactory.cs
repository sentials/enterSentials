using System;

namespace EnterSentials.Framework
{
    public class ComponentsBasedCommandFactory : ICommandFactory
    {
        private readonly IComponents components = null;

        public TCommand Get<TCommand>() where TCommand : ICommand
        { return components.Get<TCommand>(); }

        public ICommand Get(Type commandType)
        { return (ICommand) components.Get(commandType); }


        public ComponentsBasedCommandFactory(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }
    }
}
