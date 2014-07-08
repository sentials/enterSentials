using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public interface IValidatable
    {
        bool IsValid(out IEnumerable<ValidationResult> results);
        bool IsValid();
    }
}
