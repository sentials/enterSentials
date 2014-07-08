using EnterSentials.Framework.Properties;
using System;

namespace EnterSentials.Framework
{
    public static class DomainPolicy
    {
        private const string DomainSeederTypeNameFormat = "{0}DomainSeeder";

        public static readonly bool AuditableDomainObjectIsActiveOnCreation = Settings.Default.AuditableDomainObjectIsActiveOnCreation;
        

        public static DateTime GetCurrentTime()
        { return DateTime.Now.ToUniversalTime(); }

        public static string GetDomainSeederTypeNameFrom(Type migrationType)
        { return string.Format(DomainSeederTypeNameFormat, migrationType.Name); }
    }
}