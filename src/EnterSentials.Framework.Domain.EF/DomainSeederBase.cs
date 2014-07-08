using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace EnterSentials.Framework.Domain.EF
{
    public abstract class DomainSeederBase
    {
        private readonly Action<string, bool, object> executeSql = null;
        private readonly bool shouldSeedCultures = false;
        private readonly SqlDomainSeeder<Culture> cultureSeeder = null;


        protected void Sql(string sql, bool suppressTransaction = false, object anonymousArguments = null)
        { executeSql(sql, suppressTransaction, anonymousArguments); }


        protected DbMigration InitiatingMigration
        { get; private set; }


        protected virtual IEnumerable<Culture> GetCultures()
        { return Culture.GetAll(); }


        protected abstract void Seed();


        public void Seed(DbMigration migration)
        {
            Guard.AgainstNull(migration, "migration");
            InitiatingMigration = migration;

            if (shouldSeedCultures)
                cultureSeeder.Seed(GetCultures());

            Seed();
        }


        public DomainSeederBase(Action<string, bool, object> executeSql, bool shouldSeedCultures = false)
        {
            Guard.AgainstNull(executeSql, "executeSql");
            this.executeSql = executeSql;

            this.shouldSeedCultures = shouldSeedCultures;
            cultureSeeder = new SqlDomainSeeder<Culture>(executeSql, entityHasDatabaseGeneratedIdentity: false);
        }
    }
}