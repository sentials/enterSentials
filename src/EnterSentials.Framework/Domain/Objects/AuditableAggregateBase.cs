using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public abstract class AuditableAggregateBase : AggregateBase, IAuditable
    {
        [Required]
        public bool IsActive { get; set; }

        public AuditableAggregateBase()
        { IsActive = DomainPolicy.AuditableDomainObjectIsActiveOnCreation; }
    }
}