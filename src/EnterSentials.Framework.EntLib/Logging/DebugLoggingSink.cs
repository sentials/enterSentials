using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace EnterSentials.Framework.EntLib
{
    public class DebugLoggingSink : SemanticLoggingSinkBase
    {
        protected override void AttachTo(ObservableEventListener listener)
        { listener.LogToDebug(); }
    }
}