using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EnterSentials.Framework
{
    public abstract class ValidatableObject : IValidatable//, IValidatableObject
    {
        public virtual bool IsValid(out IEnumerable<ValidationResult> validationResults)
        {
            validationResults = this.Validate(new ValidationContext(this, serviceProvider: null, items: null)); 

            return validationResults == null
                || !validationResults.Any()
                || validationResults.All(r => r == ValidationResult.Success);
        }


        public bool IsValid()
        {
            var ignored = (IEnumerable<ValidationResult>) null;
            return IsValid(out ignored);
        }

        protected IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = (IEnumerable<ValidationResult>)null;
            this.IsValidByDataAnnotations(validationContext, out results);
            return results;
        }
        

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{ return Validate(validationContext); }
    }
}
