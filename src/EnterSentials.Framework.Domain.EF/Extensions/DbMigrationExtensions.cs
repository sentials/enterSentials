using EnterSentials.Framework;
using EnterSentials.Framework.Domain.EF;
using System.Data.Entity.Migrations;
using System.Linq;

namespace System
{
    public static class DbMigrationExtensions
    {
        private static bool TryGetDomainSeederTypeFrom(Type migrationType, out Type domainSeederType)
        {
            var domainSeederTypeName = DomainPolicy.GetDomainSeederTypeNameFrom(migrationType);
            
            domainSeederType = migrationType.Assembly.DefinedTypes.FirstOrDefault(type =>
                type.IsSubclassOf<DomainSeederBase>()
                && string.Equals(type.Name, domainSeederTypeName, StringComparison.InvariantCultureIgnoreCase));

            return domainSeederType != null;
        }


        public static void ShouldTrySeedingWith(this DbMigration migration, Action<string, bool, object> executeSql)
        {
            Guard.AgainstNull(migration, "migration");
            Guard.AgainstNull(executeSql, "executeSql");
            
            var domainSeederType = (Type)null;
            if (TryGetDomainSeederTypeFrom(migration.GetType(), out domainSeederType))
            {
                var seeder = Activator.CreateInstance(domainSeederType, executeSql) as DomainSeederBase;
                if (seeder != null)
                    seeder.Seed(migration);
            }
        }
    }
}
