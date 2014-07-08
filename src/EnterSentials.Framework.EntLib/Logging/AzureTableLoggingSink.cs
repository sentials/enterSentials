using EnterSentials.Framework.EntLib.Properties;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace EnterSentials.Framework.EntLib
{
    public class AzureTableLoggingSink : SemanticLoggingSinkBase
    {
        public class Parameters
        {
            public static readonly string DefaultInstanceName = Settings.Default.DefaultLoggingAzureInstanceName;
            public static readonly string DefaultStorageAccountConnectionString = Settings.Default.DefaultLoggingAzureStorageAccountConnectionString;
            public static readonly string DefaultTableName = Settings.Default.DefaultLoggingAzureTableName;

            public string InstanceName { get; private set; }
            public string StorageAccountConnectionString { get; private set; }
            public string TableName { get; private set; }

            public Parameters(
                string instanceName = null,
                string storageAccountConnectionString = null,
                string tableName = null)
            {
                InstanceName = instanceName ?? DefaultInstanceName;
                StorageAccountConnectionString = storageAccountConnectionString ?? DefaultStorageAccountConnectionString;
                TableName = tableName ?? DefaultTableName;
            }
        }

        private static readonly bool ShouldLog = Settings.Default.AllowLoggingToAzureTable;

        private readonly Parameters parameters = null;


        protected override void AttachTo(ObservableEventListener listener)
        {
            if (ShouldLog)
                listener.LogToWindowsAzureTable(
                    parameters.InstanceName,
                    parameters.StorageAccountConnectionString,
                    parameters.TableName);
        }


        public AzureTableLoggingSink(Parameters parameters)
        {
            Guard.AgainstNull(parameters, "parameters");
            this.parameters = parameters;
        }
    }
}
