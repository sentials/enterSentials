using Microsoft.WindowsAzure.Storage;
using System.Text.RegularExpressions;

namespace EnterSentials.Framework.Azure
{
    public class AzureFileRepositoryFactory : IFileRepositoryFactory
    {
        private const string ConnectionStringExpression = @"^\s*\[{1,3}\s*[\w\W]+\s*\]{1,3}\[{1,3}\s*[\w\W]+\s*\]{1,3}\s*$";
        private const string ConnectionStringSplittingExpression = @"\]{1,3}\s*\[{1,3}";
        private const RegexOptions ConnectionStringMatchingOptions = RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase;

        private readonly IAzureBlobStorageConnectionStringResolver connectionStringResolver = null;


        public IFileRepository Get(string repositoryName)
        {
            var connectionString = connectionStringResolver.GetConnectionStringFor(repositoryName);
            
            Guard.Against(
                repositoryName, 
                r => !Regex.Match(connectionString, ConnectionStringExpression, ConnectionStringMatchingOptions).Success,
                "Configured connection string for respository must be valid format.",
                "repositoryName");

            var components = Regex.Split(connectionString, ConnectionStringSplittingExpression);
            var storageUrl = components[0].TrimStart('[');
            var containerName = components[1].TrimEnd(']');
            var storageAccount = CloudStorageAccount.Parse(storageUrl);

            return new AzureFileRepository(storageAccount, containerName);
        }


        public AzureFileRepositoryFactory(IAzureBlobStorageConnectionStringResolver connectionStringResolver)
        {
            Guard.AgainstNull(connectionStringResolver, "connectionStringResolver");
            this.connectionStringResolver = connectionStringResolver;
        }
    }
}
