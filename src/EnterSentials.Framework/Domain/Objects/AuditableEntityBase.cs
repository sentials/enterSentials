using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public abstract class AuditableEntityBase<TKey> : EntityBase<TKey>, IAuditable
    {
        [Required]
        public bool IsActive { get; set; }

        public AuditableEntityBase()
        { IsActive = DomainPolicy.AuditableDomainObjectIsActiveOnCreation; }
    }


    public abstract class AuditableEntityBase : EntityBase, IAuditable
    {
        [Required]
        public bool IsActive { get; set; }

        public AuditableEntityBase()
        { IsActive = DomainPolicy.AuditableDomainObjectIsActiveOnCreation; }
    }
}