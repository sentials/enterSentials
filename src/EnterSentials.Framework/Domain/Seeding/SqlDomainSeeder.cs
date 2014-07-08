using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EnterSentials.Framework
{
    public class SqlDomainSeeder<TEntity> : DomainSeeder<TEntity>
    {
        private const string IdPropertyName = "Id";


        private static string GetColumnsEnclosureFor(IEnumerable<string> strings)
        {
            Guard.Against(strings, s => !s.Any(), "At least one string must be provided", "strings");

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("({0}", strings.First()));

            foreach (var @string in strings.Skip(1))
                stringBuilder.Append(string.Format(", {0}", @string));

            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }


        protected static string GetColumnsInsertionDeclarationFrom(IEnumerable<Column> columns)
        {
            Guard.AgainstNull(columns, "columns");
            Guard.Against(columns, c => !c.Any(), "At least one column must be provided", "columns");
            return GetColumnsEnclosureFor(columns.Select(c => c.Name));
        }


        private static string GetColumnInsertionValueFrom(object entity, Column column)
        {
            var value = column.EntityProperty.GetValue(entity);

            if ((value != null) && (column.EntityProperty.PropertyType == typeof(bool)))
                value = ((bool)value) ? 1 : 0;

            return value == null
                ? "null"
                : string.Format(column.InsertionStringFormat, value);
        }


        protected static string GetColumnsInsertionValueFrom(object entity, IEnumerable<Column> columns, bool isLastInsert)
        {
            Guard.AgainstNull(entity, "entity");
            Guard.AgainstNull(columns, "column");
            Guard.Against(columns, c => !c.Any(), "At least one column must be provided", "columns");

            var insertionValue = GetColumnsEnclosureFor(columns.Select(column => GetColumnInsertionValueFrom(entity, column)));
            return isLastInsert ? insertionValue : string.Format("{0},", insertionValue);
        }


        protected static string GetDefaultTableNameFor(Type entityType)
        { return string.Format("dbo.{0}", entityType.Name); }


        protected static IEnumerable<Column> GetDefaultColumnsFor(
            Type entityType, 
            bool entityHasDatabaseGeneratedIdentity,
            bool overrideEntityDatabaseGeneratedIdentity
        )
        {
            var properties = entityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => (!p.GetMethod.IsVirtual || p.GetMethod.IsFinal) && (p.GetGetMethod() != null) && (p.GetSetMethod() != null));

            if (entityHasDatabaseGeneratedIdentity && !overrideEntityDatabaseGeneratedIdentity)
            {
                var idProperties = properties.Where(p =>
                    string.Equals(p.Name, IdPropertyName, StringComparison.InvariantCultureIgnoreCase)
                    || (p.GetCustomAttribute(typeof(KeyAttribute), true) != null));

                if (idProperties.Any())
                    properties = properties.Except(idProperties);
            }

            return properties
                .Select(property => new Column(property))
                .ToArray();
        }

        protected static string GetIdentityInsertStatementFor(string tableName, bool onOrOff)
        { return string.Format("set identity_insert {0} {1}", tableName, onOrOff ? "on" : "off"); }


        public string DefaultTableName
        { get; private set; }

        public IEnumerable<Column> DefaultColumns
        { get; private set; }


        private readonly Action<string, bool, object> executeSql = null;
        
        private StringBuilder insertionSql = null;

        
        protected bool EntityHasDatabaseGeneratedIdentity
        { get; private set; }

        protected bool OverrideEntityDatabaseGeneratedIdentity
        { get; private set; }

        protected string TableName
        { get; private set; }

        protected IEnumerable<Column> Columns 
        { get; private set; }


        protected void ExecuteSql(string sql, bool suppressTransaction = false, object anonymousArguments = null)
        { executeSql(sql, suppressTransaction, anonymousArguments); }


        protected override void Insert(TEntity entity, bool isFirstInsert, bool isLastInsert)
        {
            if (isFirstInsert)
            {
                insertionSql.AppendLine(
                    string.Format(
                        "insert into {0} {1} values",
                        TableName,
                        GetColumnsInsertionDeclarationFrom(Columns)));
            }

            insertionSql.AppendLine(GetColumnsInsertionValueFrom(entity, Columns, isLastInsert));

            if (isLastInsert)
            {
                if (EntityHasDatabaseGeneratedIdentity && OverrideEntityDatabaseGeneratedIdentity)
                {
                    insertionSql.Insert(0, string.Format("{0}\n", GetIdentityInsertStatementFor(TableName, true)));
                    insertionSql.AppendLine(GetIdentityInsertStatementFor(TableName, false));
                }
            }
        }


        protected override void Insert(IEnumerable<TEntity> entities)
        {
            insertionSql = new StringBuilder();

            base.Insert(entities);

            if (insertionSql.Length > 0)
                ExecuteSql(insertionSql.ToString());

            insertionSql = null;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage", 
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification="This is the intended usage of the virtual property, and relatively harmless in this situation."
        )]
        public SqlDomainSeeder(
            Action<string, bool, object> executeSql,
            bool entityHasDatabaseGeneratedIdentity = true,
            bool overrideEntityDatabaseGeneratedIdentity = false,
            string tableName = null,
            IEnumerable<Column> columns = null
        )
	    {
            Guard.AgainstNull(executeSql, "executeSql");
            this.executeSql = executeSql;

            if (!entityHasDatabaseGeneratedIdentity)
                overrideEntityDatabaseGeneratedIdentity = false;

            EntityHasDatabaseGeneratedIdentity = entityHasDatabaseGeneratedIdentity;
            OverrideEntityDatabaseGeneratedIdentity = overrideEntityDatabaseGeneratedIdentity;

            DefaultTableName = GetDefaultTableNameFor(typeof(TEntity));
            DefaultColumns = GetDefaultColumnsFor(typeof(TEntity), EntityHasDatabaseGeneratedIdentity, OverrideEntityDatabaseGeneratedIdentity);
            
            TableName = tableName ?? DefaultTableName;
            Columns = columns ?? DefaultColumns;
	    }



        public class Column
        {
            public PropertyInfo EntityProperty { get; private set; }
            public string Name { get; private set; }
            public string InsertionStringFormat { get; private set; }

            private static string GetNameFrom(PropertyInfo entityProperty)
            { return entityProperty.Name; }

            private static string GetNonNullInsertionStringFormatFor(Type propertyType)
            {
                return (propertyType == typeof(string)) || (propertyType == typeof(DateTime))
                    ? "N'{0}'"
                    : "{0}";
            }

            private static string GetInsertionStringFormatFrom(PropertyInfo entityProperty)
            { return GetNonNullInsertionStringFormatFor(entityProperty.PropertyType); }

            public Column(PropertyInfo entityProperty, string name = null, string insertionStringFormat = null)
            {
                Guard.AgainstNull(entityProperty, "entityProperty");
                EntityProperty = entityProperty;

                if (string.IsNullOrEmpty(name))
                    name = GetNameFrom(entityProperty);

                if (string.IsNullOrEmpty(insertionStringFormat))
                    insertionStringFormat = GetInsertionStringFormatFrom(entityProperty);

                Name = name;
                InsertionStringFormat = insertionStringFormat;
            }

            public Column(Expression<Func<TEntity, object>> propertyAccessorExpression, string name = null, string insertionStringFormat = null)
                : this((PropertyInfo)(propertyAccessorExpression.ToMemberExpression().Member), name, insertionStringFormat)
            { }
        }
    }



    public class SqlDomainSeeder<TEntity, TEntityLocalization> : SqlDomainSeeder<TEntity>
        where TEntity : ILocalizableEntity<TEntityLocalization>
        where TEntityLocalization : IEntityLocalization
    {
        private StringBuilder insertionSql = null;
        private bool localizationsHaveBeenEncountered = false;


        public string DefaultLocalizationsTableName
        { get; private set; }

        public IEnumerable<Column> DefaultLocalizationsColumns
        { get; private set; }

        protected string LocalizationsTableName
        { get; private set; }

        protected IEnumerable<Column> LocalizationsColumns
        { get; private set; }

        protected bool LocalizationEntityHasDatabaseGeneratedIdentity
        { get; private set; }

        protected bool OverrideLocalizationEntityDatabaseGeneratedIdentity
        { get; private set; }


        protected override void Insert(TEntity entity, bool isFirstInsert, bool isLastInsert)
        {
            base.Insert(entity, isFirstInsert, isLastInsert);

            var localizations = entity.Localizations;

            if ((localizations != null) && (localizations.Any()))
            {
                if (!localizationsHaveBeenEncountered)
                {
                    localizationsHaveBeenEncountered = true;
                    insertionSql.AppendLine(
                        string.Format(
                            "insert into {0} {1} values",
                            LocalizationsTableName,
                            GetColumnsInsertionDeclarationFrom(LocalizationsColumns)));
                }

                var lastLocalizationInsertIndex = localizations.Count() - 1;
                var localizationInsertIndex = 0;
                foreach (var localization in localizations)
                {
                    var isLastLocalizationInsert = isLastInsert && (localizationInsertIndex == lastLocalizationInsertIndex);
                    insertionSql.AppendLine(GetColumnsInsertionValueFrom(localization, LocalizationsColumns, isLastLocalizationInsert));
                    localizationInsertIndex++;
                }
            }

            if (isLastInsert)
            {
                if (localizationsHaveBeenEncountered)
                {
                    if (LocalizationEntityHasDatabaseGeneratedIdentity && OverrideLocalizationEntityDatabaseGeneratedIdentity)
                    {
                        insertionSql.Insert(0, string.Format("{0}\n", GetIdentityInsertStatementFor(LocalizationsTableName, true)));
                        insertionSql.AppendLine(GetIdentityInsertStatementFor(LocalizationsTableName, false));
                    }
                }
            }
        }


        protected override void Insert(IEnumerable<TEntity> entities)
        {
            insertionSql = new StringBuilder();

            base.Insert(entities);

            if (insertionSql.Length > 0)
                ExecuteSql(insertionSql.ToString());

            insertionSql = null;
        }


        public SqlDomainSeeder(
            Action<string, bool, object> executeSql,
            bool entityHasDatabaseGeneratedIdentity = true,
            string tableName = null,
            IEnumerable<Column> columns = null,
            bool localizationEntityHasDatabaseGeneratedIdentity = true,
            bool overrideLocalizationEntityDatabaseGeneratedIdentity = false,
            string localizationsTableName = null,
            IEnumerable<Column> localizationsColumns = null
        ) : base (executeSql, entityHasDatabaseGeneratedIdentity, true, tableName, columns)
        {
            if (!localizationEntityHasDatabaseGeneratedIdentity)
                overrideLocalizationEntityDatabaseGeneratedIdentity = false;

            LocalizationEntityHasDatabaseGeneratedIdentity = localizationEntityHasDatabaseGeneratedIdentity;
            OverrideLocalizationEntityDatabaseGeneratedIdentity = overrideLocalizationEntityDatabaseGeneratedIdentity;

            DefaultLocalizationsTableName = GetDefaultTableNameFor(typeof(TEntityLocalization));
            DefaultLocalizationsColumns = GetDefaultColumnsFor(typeof(TEntityLocalization), LocalizationEntityHasDatabaseGeneratedIdentity, OverrideLocalizationEntityDatabaseGeneratedIdentity);

            LocalizationsTableName = localizationsTableName ?? DefaultLocalizationsTableName;
            LocalizationsColumns = localizationsColumns ?? DefaultLocalizationsColumns;
        }
    }
}