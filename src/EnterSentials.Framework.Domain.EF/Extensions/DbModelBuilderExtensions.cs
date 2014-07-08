using EnterSentials.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;

namespace EnterSentials.Framework.Domain.EF
{
    public static class DbModelBuilderExtensions
    {
        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        private static MethodInfo GetConfigurationRegistrarAddMethodWithParameter(Type configurationType)
        {
            Guard.AgainstNull(configurationType, "configurationType");
            return typeof(ConfigurationRegistrar).GetMethods().First(method =>
            {
                var isOrNot = string.Equals(method.Name, "Add");
                if (isOrNot)
                {
                    var parameters = method.GetParameters();
                    isOrNot = parameters.First().ParameterType.GetGenericTypeDefinition() == configurationType;
                }
                return isOrNot;
            });
        }


        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        private static readonly MethodInfo AddEntityTypeConfigurationMethod =
            GetConfigurationRegistrarAddMethodWithParameter(typeof(EntityTypeConfiguration<>));


        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        private static readonly MethodInfo AddComplexTypeConfigurationMethod =
            GetConfigurationRegistrarAddMethodWithParameter(typeof(ComplexTypeConfiguration<>));

        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static void AddConfigurationsDefinedWithin(this DbModelBuilder dbModelBuilder, Type type)
        {
            Guard.AgainstNull(dbModelBuilder, "dbModelBuilder");
            Guard.AgainstNull(type, "type");

            foreach (var configurationType in type
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(nestedType => nestedType.IsComplexTypeConfiguration()))
            {
                AddComplexTypeConfigurationMethod.MakeGenericMethod(configurationType.BaseType.GenericTypeArguments.First())
                    .Invoke(dbModelBuilder.Configurations, new object[] { Activator.CreateInstance(configurationType) });
            }


            foreach (var configurationType in type
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(nestedType => nestedType.IsEntityTypeConfiguration()))
            {
                AddEntityTypeConfigurationMethod.MakeGenericMethod(configurationType.BaseType.GenericTypeArguments.First())
                    .Invoke(dbModelBuilder.Configurations, new object[] { Activator.CreateInstance(configurationType) });
            }
        }

        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static void AddConfigurationsDefinedWithin(this DbModelBuilder dbModelBuilder, object @object)
        {
            Guard.AgainstNull(@object, "object");
            dbModelBuilder.AddConfigurationsDefinedWithin(@object.GetType());
        }
    }
}
