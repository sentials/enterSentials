using System.ComponentModel.DataAnnotations;
using Id = System.Int32;

namespace EnterSentials.Framework
{
    public interface IEntity<TId> : IDomainObject
    {
        [Key]
        TId Id { get; set; }
    }


    public interface IEntity : IEntity<Id>
    { }
}