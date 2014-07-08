using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace EnterSentials.Framework.EntLib
{
    public class ConsoleLoggingSink : SemanticLoggingSinkBase
    {
        protected override void AttachTo(ObservableEventListener listener)
        { listener.LogToConsole(); }
    }
}
