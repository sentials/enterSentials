using System;
using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredAndNonDefaultAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        { 
            return base.IsValid(value) 
                && (((value is string) && (value != null)) || value != Activator.CreateInstance(value.GetType())); 
        }
    }
}
