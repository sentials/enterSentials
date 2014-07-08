using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework.EntLib
{
    public abstract class SemanticLoggingSinkBase : ScopeLimitedObject, ILoggingSink
    {
        protected abstract void AttachTo(ObservableEventListener listener);

        public void AttachTo(EventListener listener)
        {
            var observableEventListener = listener as ObservableEventListener;
            if (observableEventListener != null)
                AttachTo(observableEventListener);
        }
    }
}