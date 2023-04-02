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

        //Accounts Microservice Entities
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountCreationStatus> AccountCreationStatuses { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK_Account");

                /*entity.HasOne(d => d.AccountType)
                    .*/

                entity.Property(e => e.DateCreated).HasDefaultValue(DateTime.Now);

                entity.HasCheckConstraint("Balance_check", "Balance >= 0");


            });


            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.HasKey(e => e.AccountTypeId).HasName("PK_Account_Type");

                entity.Property(e => e.Type).HasMaxLength(7);
            });

            modelBuilder.Entity<AccountCreationStatus>(entity =>
            {
                    entity.HasKey(e => e.AccountCreationStatusId).HasName("PK_Account_Creation_Status");

                    entity.Property(e => e.Message).HasMaxLength(50);
            });

            modelBuilder.Entity<Statement>(entity =>
            {
                entity.HasKey(e => e.StatementId).HasName("PK_Statement_ID");

                /*account id foreign key*/
                
                entity.Property(e => e.Narration).HasMaxLength(35);

                entity.HasCheckConstraint("Withdrawal_Check", "Withdrawal >= 0");
          

            });

            modelBuilder.Entity<TransactionStatus>(entity =>
            {
                entity.HasKey(e => e.TransactionStatusId).HasName("PK_Transaction_Status");

                //account id foreign key

                entity.Property(e => e.Message).HasMaxLength(35);
            });
        }
    


    }
}