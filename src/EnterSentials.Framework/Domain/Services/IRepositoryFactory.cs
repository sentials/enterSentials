namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public interface IRepositoryFactory
    {
        IRepository<T> Get<T>() where T : class;
        IReadOnlyRepository<T> GetReadOnly<T>() where T : class;
    }
}
