using System;
using System.Collections;

namespace EnterSentials.Framework
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredAndNonDefaultNorEmptyAttribute : RequiredAndNonDefaultAttribute
    {
        public override bool IsValid(object value)
        {
            var baseIsValid = base.IsValid(value);
            var isNonStringOrValidString = true;
            var isNonEnumerableOrNonEmptyEnumerable = true;

            if (baseIsValid)
            {
                if (value is string)
                    isNonStringOrValidString = !string.IsNullOrEmpty(((string)value).Trim());
                else
                {
                    var manyObjects = (IEnumerable)null;
                    if (value.TryAsMany(out manyObjects))
                        isNonEnumerableOrNonEmptyEnumerable = manyObjects.GetEnumerator().MoveNext();
                }
            }

            return baseIsValid && isNonStringOrValidString && isNonEnumerableOrNonEmptyEnumerable;
        }
    }
}
