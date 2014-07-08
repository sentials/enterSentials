namespace EnterSentials.Framework.Azure
{
    public interface IAzureBlobStorageConnectionStringResolver
    {
        string GetConnectionStringFor(string fileRepositoryName);
    }
}