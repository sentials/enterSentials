using System;
using System.Linq;
using System.Reflection;

namespace EnterSentials.Framework
{
    public static class PropertyInfoExtensions
    {
        private static Func<PropertyInfo, Type, bool, bool> PropertyHasAttributeOfType =
            (property, attributeType, searchInheritanceChain) => property.GetCustomAttributes(attributeType, searchInheritanceChain).Any();

        private static Func<PropertyInfo, Type, bool> PropertyHasDeclaredAttributeOfType =
            (property, attributeType) => PropertyHasAttributeOfType(property, attributeType, false);

        private static Func<PropertyInfo, Type, bool> PropertyHasDeclaredOrInheritedAttributeOfType =
            (property, attributeType) => PropertyHasAttributeOfType(property, attributeType, true);


        public static bool HasDeclaredOrInheritedAttributeOfType(this PropertyInfo property, Type attributeType)
        { return PropertyHasDeclaredOrInheritedAttributeOfType(property, attributeType); }
    }
}
