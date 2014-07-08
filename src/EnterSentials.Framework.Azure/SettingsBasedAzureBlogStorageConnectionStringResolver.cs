using EnterSentials.Framework.Azure.Properties;

namespace EnterSentials.Framework.Azure
{
    public class SettingsBasedAzureBlogStorageConnectionStringResolver : IAzureBlobStorageConnectionStringResolver
    {
        private const string SettingsPropertyNameFormat = "{0}AzureBlobStorageConnectionString";
        
        private static readonly string DefaultConnectionStringSettingsPropertyName = GetSettingsPropertyNameFrom("Default");
        

        private static string GetSettingsPropertyNameFrom(string fileRepositoryName)
        { return string.Format(SettingsPropertyNameFormat, fileRepositoryName); }


        public string GetConnectionStringFor(string fileRepositoryName)
        { return Settings.Default[GetSettingsPropertyNameFrom(fileRepositoryName)].ToString(); }
    }
}