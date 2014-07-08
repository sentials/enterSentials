using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    public abstract class NameKeyedLocalizableEntity<TEntityLocalization> : NameKeyedEntityBase, ILocalizableEntity<TEntityLocalization> where TEntityLocalization : IEntityLocalization
    {
        private readonly ILocalizationsManager<TEntityLocalization> localizationsManager = null;


        public virtual ICollection<TEntityLocalization> Localizations { get; set; }


        public bool TryGetLocalizedFor(string cultureName, out TEntityLocalization localization)
        { return localizationsManager.TryGetLocalizedFor(cultureName, out localization); }

        public TEntityLocalization LocalizedFor(string cultureName)
        { return localizationsManager.LocalizedFor(cultureName); }

        public bool IsLocalizedFor(string cultureName)
        { return localizationsManager.IsLocalizedFor(cultureName); }


        public NameKeyedLocalizableEntity()
        { localizationsManager = new LocalizationsManager<TEntityLocalization>(this); }
    }
}
