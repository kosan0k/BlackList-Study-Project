using System.Data.Entity.Migrations;

namespace BlackList.Storage.Sql
{
    public sealed class Configuration : DbMigrationsConfiguration<RepositoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Project.Infrastructure.MyDbContext";
        }
    }
}
