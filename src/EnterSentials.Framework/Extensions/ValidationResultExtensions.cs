using System;
using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public static class ValidationResultExtensions
    {
        public static ValidationException ToException(this ValidationResult validationResult)
        { return new ValidationException(validationResult.ErrorMessage); }

        public static ValidationException ToException(this ValidationResult validationResult, Exception innerException)
        { return new ValidationException(validationResult.ErrorMessage, innerException); }
    }
}
