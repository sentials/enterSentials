namespace EnterSentials.Framework
{
    public interface IFilter
    {
        TTarget Process<TTarget>(TTarget target);
    }
}