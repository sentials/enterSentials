namespace EnterSentials.Framework
{
    public interface IFileRepositoryFactory
    {
        IFileRepository Get(string repositoryName);
    }
}