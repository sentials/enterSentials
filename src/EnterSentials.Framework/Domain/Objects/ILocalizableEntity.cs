using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface ILocalizableEntity<TEntityLocalization> : ILocalizationsManager<TEntityLocalization> where TEntityLocalization : IEntityLocalization
    {
        ICollection<TEntityLocalization> Localizations { get; set; }
    }
}