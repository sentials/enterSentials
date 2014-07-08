using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public abstract class AuditableNameKeyedEntityBase : NameKeyedEntityBase, IAuditable
    {
        [Required]
        public bool IsActive { get; set; }

        public AuditableNameKeyedEntityBase()
        { IsActive = DomainPolicy.AuditableDomainObjectIsActiveOnCreation; }
    }
}