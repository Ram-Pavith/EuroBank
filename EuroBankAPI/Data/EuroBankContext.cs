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

       
          

            modelBuilder.Entity<AccountCreationStatus>(entity =>
            {
                entity.HasKey(e => e.AccountCreationStatusId).HasName("PK_Account_Creation_Status");

                entity.Property(e => e.Message).HasMaxLength(50);
            });

            modelBuilder.Entity<Statement>(entity =>
            {
                entity.HasKey(e => e.StatementId).HasName("PK_Statement_ID");

                entity.Property(e => e.Narration).HasMaxLength(35);


            });
        }


    }
}
