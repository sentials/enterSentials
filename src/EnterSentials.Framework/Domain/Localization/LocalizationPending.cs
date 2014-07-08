namespace EnterSentials.Framework
{
    public class LocalizationPending<TEntity>
    {
        public TEntity Entity { get; private set; }
        public string CultureName { get; private set; }

        public LocalizationPending(TEntity entity, string cultureName)
        {
            Guard.AgainstNull(entity, "entity");
            Guard.AgainstNullOrEmpty(cultureName, "cultureName");
            Entity = entity;
            CultureName = cultureName;
        }
    }
}