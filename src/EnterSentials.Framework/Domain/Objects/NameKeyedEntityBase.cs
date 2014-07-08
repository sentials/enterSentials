using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterSentials.Framework
{
    public class NameKeyedEntityBase : DomainObjectBase
    {
        [Key]
        public string Name { get; set; }        
    }
}