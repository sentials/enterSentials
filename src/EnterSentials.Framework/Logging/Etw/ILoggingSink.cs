using System;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public interface ILoggingSink : IDisposable
    {
        void AttachTo(EventListener listener);
    }
}