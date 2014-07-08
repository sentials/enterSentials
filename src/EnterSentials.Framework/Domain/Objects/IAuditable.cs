using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public interface IAuditable
    {
        [Required]
        bool IsActive { get; set; }
    }
}