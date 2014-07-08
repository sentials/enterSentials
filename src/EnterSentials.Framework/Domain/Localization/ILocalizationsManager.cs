namespace EnterSentials.Framework
{
    public interface ILocalizationsManager<TEntityLocalization> where TEntityLocalization : IEntityLocalization
    {
        bool TryGetLocalizedFor(string cultureName, out TEntityLocalization localization);
        TEntityLocalization LocalizedFor(string cultureName);
        bool IsLocalizedFor(string cultureName);
    }
}