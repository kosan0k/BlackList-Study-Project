using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace BlackList.Storage.Sql
{
    public class DbInitializer : CreateDatabaseIfNotExists<RepositoryContext>
    {
        public override void InitializeDatabase(RepositoryContext context)
        {
            DbMigrator dbMigrator = new DbMigrator(new Configuration
            {                
                TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "Npgsql")
            });
        }
    }
}
