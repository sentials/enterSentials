using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnterSentials.Framework
{
    public static class TypeExtensions
    {
        private static readonly Type ObjectType = typeof(object);

        private static readonly IEnumerable<string> DynamicProxyNamespaces = new string[]
        {
            "System.Data.Entity.DynamicProxies"
        };


        public static bool IsSubclassOf<T>(this Type type)
        { return type.IsSubclassOf(typeof(T)); }

        public static bool IsAssignableFrom<T>(this Type type)
        { return type.IsAssignableFrom<T>(); }

        public static bool Implements(this Type type, Type interfaceType)
        { return interfaceType.IsAssignableFrom(type); }

        public static bool Implements<TInterface>(this Type type)
        { return typeof(TInterface).IsAssignableFrom(type); }


        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static bool IsSubclassOfGenericTypeWithDefinition(this Type type, Type genericTypeDefinition)
        {
            Guard.AgainstNull(type, "type");
            Guard.Against(genericTypeDefinition, gtd => !gtd.IsGenericTypeDefinition, "Must be a generic type definition", "genericTypeDefinition");
            var isOrNot = false;
            var baseType = type.BaseType;

            while (baseType != null && !isOrNot)
            {
                if (baseType.IsGenericType)
                    isOrNot = (baseType.GetGenericTypeDefinition() == genericTypeDefinition);
                baseType = baseType.BaseType;
            }

            return isOrNot;
        }


        public static IEnumerable<PropertyInfo> PropertiesWithDeclaredOrInheritedAttributeOfType(this Type type, Type attributeType)
        { return type.GetProperties().Where(property => property.HasDeclaredOrInheritedAttributeOfType(attributeType)); }


        public static string GetConfigurationKey(this Type type)
        { return string.Format("{0}, {1}", type.FullName, new AssemblyName(type.Assembly.FullName).Name); }


        public static bool IsGenericType(this Type type, Type genericTypeDefinition)
        {
            return (type != null)
                && (type.IsAbstract)
                && (type.IsGenericType)
                && (type.GetGenericArguments().Count() == genericTypeDefinition.GetGenericArguments().Count())
                && (type.Name.StartsWith(genericTypeDefinition.Name));
        }


        public static bool IsFulfilledGenericType(this Type type, Type genericTypeDefinition, out IEnumerable<Type> genericArguments)
        {
            genericArguments = Type.EmptyTypes;

            var isOrNot = (type != null) && type.IsGenericType(genericTypeDefinition) && !type.IsGenericTypeDefinition;

            if (isOrNot)
                genericArguments = type.GetGenericArguments();

            return isOrNot;
        }


        public static bool ImplementsGenericType(this Type type, Type genericTypeDefinition, out IEnumerable<Type> genericArguments)
        {
            Guard.AgainstNull(type, "type");

            genericArguments = Type.EmptyTypes;

            var doesOrNot = false;

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsFulfilledGenericType(genericTypeDefinition, out genericArguments))
                {
                    doesOrNot = true;
                    break;
                }
            }

            return doesOrNot;
        }


        public static bool InheritsFromGenericType(this Type type, Type genericTypeDefinition, out IEnumerable<Type> genericArguments)
        {
            Guard.AgainstNull(type, "type");

            genericArguments = Type.EmptyTypes;
            
            var doesOrNot = false;

            var baseType = type.BaseType;
            while (baseType != ObjectType)
            {
                if (baseType.IsFulfilledGenericType(genericTypeDefinition, out genericArguments))
                {
                    doesOrNot = true;
                    break;
                }
                baseType = baseType.BaseType;
            }

            return doesOrNot;
        }


        public static bool InheritsFromObjectCopierBase(this Type type, out Type fromType, out Type toType)
        {
            Guard.AgainstNull(type, "type");

            var genericArguments = (IEnumerable<Type>)null;

            var doesOrNot = type.InheritsFromGenericType(typeof(ObjectCopierBase<,>), out genericArguments);

            fromType = doesOrNot ? genericArguments.ElementAt(0) : null;
            toType = doesOrNot ? genericArguments.ElementAt(1) : null;

            return doesOrNot;
        }


        public static bool InheritsFromObjectTranslatorBase(this Type type, out Type fromType, out Type toType)
        {
            Guard.AgainstNull(type, "type");

            var genericArguments = (IEnumerable<Type>)null;

            var doesOrNot = type.InheritsFromGenericType(typeof(ObjectTranslatorBase<,>), out genericArguments);

            fromType = doesOrNot ? genericArguments.ElementAt(0) : null;
            toType = doesOrNot ? genericArguments.ElementAt(1) : null;

            return doesOrNot;
        }


        public static bool InheritsFromObjectCopierBase(this Type type)
        {
            var ignored1 = (Type)null;
            var ignored2 = (Type)null;
            return type.InheritsFromObjectCopierBase(out ignored1, out ignored2);
        }

        public static bool InheritsFromObjectTranslatorBase(this Type type)
        {
            var ignored1 = (Type)null;
            var ignored2 = (Type)null;
            return type.InheritsFromObjectTranslatorBase(out ignored1, out ignored2);
        }


        public static bool IsDynamicProxyType(this Type type)
        { return DynamicProxyNamespaces.Contains(type.Namespace); }
    }
}