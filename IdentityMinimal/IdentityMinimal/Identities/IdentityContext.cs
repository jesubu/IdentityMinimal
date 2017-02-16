using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityMinimal.Identities
{
    public class IdentityContext : IdentityDbContext<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public IdentityContext(string connString)
            : base(connString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }
        public IdentityContext()
            : base("KatarinaIdentityDb")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }
        
        //public DbSet<Role> Applicationroles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            
            base.OnModelCreating(modelBuilder);
        }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }

        //public void InitializeDataStore()
        //{
        //    System.Data.Entity.Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<IdentityContext, Configuration>());

        //    var configuration = new Configuration();
        //    var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
        //    if (migrator.GetPendingMigrations().Any())
        //    {
        //        migrator.Update();
        //    }
        //}
    }
}