using System.Linq;

namespace EnterSentials.Framework
{
    public class LocalizationsManager<TEntityLocalization> : ILocalizationsManager<TEntityLocalization> where TEntityLocalization : IEntityLocalization
    {
        private readonly ILocalizableEntity<TEntityLocalization> localizableEntity = null;


        public bool TryGetLocalizedFor(string cultureName, out TEntityLocalization localization)
        { 
            localization = localizableEntity.Localizations.FirstOrDefault(l => l.CultureName == Culture.For(cultureName).Name);
            return localization != null;
        }

        public TEntityLocalization LocalizedFor(string cultureName)
        {
            var localized = default(TEntityLocalization);
            Guard.Against(
                !TryGetLocalizedFor(cultureName, out localized),
                "Localization must exist for provided culture."
            );
            return localized;
        }

        public bool IsLocalizedFor(string cultureName)
        {
            var ignored = default(TEntityLocalization);
            return TryGetLocalizedFor(cultureName, out ignored);
        }


        public LocalizationsManager(ILocalizableEntity<TEntityLocalization> localizableEntity)
        {
            Guard.AgainstNull(localizableEntity, "localizableEntity");
            this.localizableEntity = localizableEntity;
        }
    }
}
