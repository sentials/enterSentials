using System.ComponentModel.DataAnnotations;
using Id = System.Int32;

namespace EnterSentials.Framework
{
    public abstract class EntityBase<TId> : DomainObjectBase, IEntity<TId>
    {
        [Key]
        public TId Id { get; set; }
    }


    public abstract class EntityBase : EntityBase<Id>, IEntity
    { }
}