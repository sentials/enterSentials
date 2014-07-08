using System;
using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class IdAttribute : RequiredAndNonDefaultNorEmptyAttribute
    {
        public override bool IsValid(object value)
        {
            return base.IsValid(value) &&
                value is int
                    ? ((int)value) > 0
                    : value is long
                        ? ((long)value) > 0
                        : true;
        }
    }
}
