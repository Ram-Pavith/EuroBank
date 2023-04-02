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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK_Account_ID");
                entity.Property(e => e.AccountId).HasMaxLength(36);

                entity.Property(e => e.DateCreated).HasDefaultValue(DateTime.Now);

                entity.HasCheckConstraint("Balance_check",
                                        "Balance >= 0");

                //foreign key

            });


            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.HasKey(e => e.AccountTypeId).HasName("PK_Account_Type");

                entity.Property(e => e.Type).HasMaxLength(7);   // "Savings" or "Current"               
            });

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
