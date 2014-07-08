using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterSentials.Framework
{
    public abstract class EntityLocalization : EntityBase, IEntityLocalization
    {
        [RequiredAndNonDefaultNorEmpty]
        [ForeignKey("Culture")]
        public string CultureName { get; set; }


        public virtual Culture Culture { get; set; }
    }
}
