namespace EnterSentials.Framework
{
    public interface IDomainFactory
    {
        TDomain Get<TDomain>() where TDomain : IDomain;
    }
}