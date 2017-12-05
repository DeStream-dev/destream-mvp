using DeStream.Common;
using DeStream.Web.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Data.Services
{
    internal class DeStreamWebDbContext:IdentityDbContext<ApplicationUser>
    {
        public const string ConnectionName = "WebDbConnection";
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserTarget> UserTargets { get; set; }
        public DbSet<UserTargetDonation> UserTargetDonations { get; set; }

        public DeStreamWebDbContext(string connectionName):base(connectionName)
        {
            Database.Log = x =>
            {
                System.Diagnostics.Debug.WriteLine(x);
            };
        }

        public DeStreamWebDbContext():base(ConnectionName)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer(new SQLite.CodeFirst.SqliteCreateDatabaseIfNotExists<DeStreamWebDbContext>(modelBuilder, true));
            modelBuilder.Entity<ApplicationUser>().HasOptional(x => x.UserProfile).WithRequired(x => x.ApplicationUser);
            modelBuilder.Entity<ApplicationUser>().HasIndex(x => x.Email).IsUnique(true);
            //modelBuilder.Entity<UserTargetDonation>().HasRequired(x=>x.)
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            modelBuilder.Properties<decimal>().Configure(x => x.HasPrecision(Constants.DefaultDecimalPrecision, Constants.DefaultDecimalScale));
            modelBuilder.Entity<UserProfile>().HasKey(x => x.ApplicationUserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
