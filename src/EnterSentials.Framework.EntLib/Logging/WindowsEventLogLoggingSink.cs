using EnterSentials.Framework.EntLib.Properties;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace EnterSentials.Framework.EntLib
{
    public class WindowsEventLogLoggingSink : SemanticLoggingSinkBase
    {
        public class Parameters
        {
            public const string DefaultLogName = Naming.ProductIdentifier;
            public const string DefaultSourceName = Naming.ProductIdentifier;
            public const string DefaultMachineName = ".";

            public string LogName { get; private set; }
            public string SourceName { get; private set; }
            public string MachineName { get; private set; }

            public Parameters(
                string logName = DefaultLogName, 
                string sourceName = DefaultSourceName,
                string machineName = DefaultMachineName)
            {
                Guard.AgainstNullOrEmpty(logName, "logName");
                Guard.AgainstNullOrEmpty(sourceName, "sourceName");
                Guard.AgainstNullOrEmpty(machineName, "machineName");
                LogName = logName;
                SourceName = sourceName;
                MachineName = machineName;
            }
        }


        private static readonly bool ShouldLog = Settings.Default.AllowLoggingToWindowsEventLog;

        private readonly Parameters parameters = null;


        protected override void AttachTo(ObservableEventListener listener)
        {
            if (ShouldLog)
                listener.LogToWindowsEventLog(
                    parameters.LogName,
                    parameters.SourceName,
                    parameters.MachineName); 
        }


        public WindowsEventLogLoggingSink(Parameters parameters)
        {
            Guard.AgainstNull(parameters, "parameters");
            this.parameters = parameters;
        }
    }
}