using System;

namespace EnterSentials.Framework
{
    public interface ICommandFactory
    {
        TCommand Get<TCommand>() where TCommand : ICommand;
        ICommand Get(Type commandType); 
    }
}