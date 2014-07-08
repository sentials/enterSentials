using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public interface IEntityLocalization
    {
        [Key]
        int Id { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        string CultureName { get; set; }


        Culture Culture { get; set; }
    }
}