using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EnterSentials.Framework
{
    public static class MethodInfoExtensions
    {
        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static bool IsLinqOperator(this MethodInfo method)
        {
            return ((method.DeclaringType == typeof(Queryable)) || (method.DeclaringType == typeof(Enumerable)))
                && (Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null);
        }
    }
}
