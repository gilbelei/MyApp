using MyApp.Domain.Entities;
using MyApp.Infra.Data.EntityConfig;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace MyApp.Infra.Data.Context
{
    public class MyAppContext : DbContext
    {
        public MyAppContext() : base("MyAppContext")
        {

        }

        public DbSet<GitHubResult> GitHubResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Do not put table names in plural
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Do not perform cascade deletion
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Place columns with the term Id as the primary key
            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(P => P.IsKey());

            //// Put string columns as varchar
            //modelBuilder.Properties<string>()
            //    .Configure(p => p.HasColumnType("varchar"));

            //// Place string type columns with size 255 when there is no explicit size
            //modelBuilder.Properties<string>()
            //    .Configure(p => p.HasMaxLength(255));

            //Set entity settings
            modelBuilder.Configurations.Add(new GitHubResultConfig());

        }

        public override int SaveChanges()
        {
            //Checks whether the InsertedAt column exists
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("InsertedAt") != null))
            {
                //When adding
                if (entry.State == EntityState.Added)
                {
                    //Enter the value of with the current date and time
                    entry.Property("InsertedAt").CurrentValue = DateTime.Now;
                }
                //When modifying
                if (entry.State == EntityState.Modified)
                {
                    //Do not change the value contained in the InsertedAt column
                    entry.Property("InsertedAt").IsModified = false;
                }
            }
            //Checks whether the UpdatedAt column exists
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("UpdatedAt") != null))
            {
                //When adding
                if (entry.State == EntityState.Modified)
                {
                    //Enter the value of with the current date and time
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
                //When modifying
                if (entry.State == EntityState.Added)
                {
                    //Do not change the value contained in the UpdatedAt column
                    entry.Property("UpdatedAt").IsModified = false;
                }
            }
            return base.SaveChanges();

        }
    }
}
