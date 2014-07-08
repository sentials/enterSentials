using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace EnterSentials.Framework.Domain.EF
{
    public class ConventionBasedSqlGenerator : SqlServerMigrationSqlGenerator
    {
        private const string GetUtcDateFunctionCall = "GETUTCDATE()";


        private static void InitializeColumnByConvention(PropertyModel column)
        {
            // Apply UTC time to relevant date colums
            if ((string.Equals(column.Name, Framework.Name.Of.Property.On<ICreationDatePersisting>(e => e.CreatedOn)))
                || (string.Equals(column.Name, Framework.Name.Of.Property.On<ILastModifiedDatePersisting>(e => e.LastModifiedOn))))
                column.DefaultValueSql = GetUtcDateFunctionCall;
            // Assume all deactivatable entities start activated
            else if (string.Equals(column.Name, Framework.Name.Of.Property.On<IAuditable>(e => e.IsActive)))
                column.DefaultValue = DomainPolicy.AuditableDomainObjectIsActiveOnCreation;
        }


        private static void InitializeColumnsByConvention(IEnumerable<ColumnModel> columns)
        { columns.ForEach(InitializeColumnByConvention); }

        
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            InitializeColumnByConvention(addColumnOperation.Column);
            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            InitializeColumnsByConvention(createTableOperation.Columns);
            base.Generate(createTableOperation);
        }
    }
}
