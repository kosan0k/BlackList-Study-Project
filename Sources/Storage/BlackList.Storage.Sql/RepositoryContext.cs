using BlackList.Storage.Sql.Entities;
using System;
using System.Data.Entity;

namespace BlackList.Storage.Sql
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base()
        { }

        public RepositoryContext(string connectionString)
            : base(connectionString)
        { }

        public virtual DbSet<PersonEntity> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
