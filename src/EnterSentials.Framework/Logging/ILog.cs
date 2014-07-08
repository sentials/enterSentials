using System;

namespace EnterSentials.Framework
{
    public interface ILog
    {
        void CriticalError(string message);
        void Error(string message);
        void Warning(string message);
        void Message(string message);
        void ExceptionByPolicy(string message, Guid id);
    }
}