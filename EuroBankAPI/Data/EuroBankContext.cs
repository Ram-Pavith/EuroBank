using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Data
{
    public class EuroBankContext : DbContext
    {
        public EuroBankContext() { }
        public EuroBankContext(DbContextOptions<EuroBankContext> options) : base(options) { }

        //DbSet Tables For the Context
        public DbSet<UserAuth> UsersAuth { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCreationStatus> CustomerCreationStatuses { get; set; }

        //Context Configuring

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;");
        //    }
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasMaxLength(10);


            });
        }
    }
}
