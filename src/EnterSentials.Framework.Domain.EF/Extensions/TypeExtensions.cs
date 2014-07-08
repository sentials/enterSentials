using System;
using System.Data.Entity.ModelConfiguration;
using EnterSentials.Framework;

namespace EnterSentials.Framework.Domain.EF
{
    public static class TypeExtensions
    {
        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static bool IsEntityTypeConfiguration(this Type type)
        { return type.IsSubclassOfGenericTypeWithDefinition(typeof(EntityTypeConfiguration<>)); }


        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static bool IsComplexTypeConfiguration(this Type type)
        { return type.IsSubclassOfGenericTypeWithDefinition(typeof(ComplexTypeConfiguration<>)); }
    }
}
