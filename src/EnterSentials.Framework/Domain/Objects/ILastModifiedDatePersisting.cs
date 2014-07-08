using System;
using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public interface ILastModifiedDatePersisting
    {
        [Required]
        DateTime LastModifiedOn { get; set; }
    }
}
