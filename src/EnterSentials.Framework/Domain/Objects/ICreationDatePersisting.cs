using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterSentials.Framework
{
    public interface ICreationDatePersisting
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        DateTime CreatedOn { get; set; }
    }
}
