namespace EnterSentials.Framework
{
    public interface IDomain
    {
        IRepositoryFactory Repositories
        { get; }
    }
}