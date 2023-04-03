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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;");
        //    }
        //}
        //Accounts Microservice Entities
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountCreationStatus> AccountCreationStatuses { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCreationStatus> CustomerCreationStatuses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Transaction> Transactions { get; set;}
        public DbSet<RefPaymentMethod> RefPaymentMethods { get; set; }
        public DbSet<RefTransactionStatus> RefTransactionStatuses { get; set; }
        public DbSet<RefTransactionType> RefTransactionType { get; set; }
        public DbSet<CounterParty> CounterParties { get; set; }
        public DbSet<Models.Service> Services { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK_Account");

                entity.HasOne(at => at.AccountType)
                        .WithMany(a => a.Accounts)
                        .HasForeignKey(at => at.AccountTypeId)
                        .IsRequired();

                entity.HasOne(ci => ci.Customer)
                        .WithMany(a => a.Accounts)
                        .HasForeignKey(ci => ci.CustomerId)
                        .IsRequired();

                entity.Property(e => e.DateCreated).HasDefaultValue(DateTime.Now);

                entity.HasCheckConstraint("Balance_check", "Balance >= 0");
                entity.Property(e => e.Balance).IsRequired();
            });


            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.HasKey(e => e.AccountTypeId).HasName("PK_Account_Type");

                entity.Property(e => e.Type).HasMaxLength(7).IsRequired();    //"Savings" or "Current"
            });

            modelBuilder.Entity<AccountCreationStatus>(entity =>
            {
                entity.HasKey(e => e.AccountCreationStatusId).HasName("PK_Account_Creation_Status");

                entity.HasOne(a => a.Account)
                        .WithOne(acs => acs.AccountCreationStatus)
                        .HasForeignKey<Account>(a => a.AccountId);

                entity.Property(e => e.Message).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Statement>(entity =>
            {
                entity.HasKey(e => e.StatementId).HasName("PK_Statement_ID");

                entity.HasOne(s => s.Account)
                    .WithOne(a => a.Statement)
                    .HasForeignKey<Account>(a => a.AccountId);

                entity.Property(e => e.Date).IsRequired();

                entity.Property(e => e.Narration).HasMaxLength(35).IsRequired();

                entity.Property(e => e.RefNo).IsRequired();

                entity.Property(e => e.ValueDate).IsRequired();
                
                entity.HasCheckConstraint("Withdrawal_Check", "Withdrawal >= 0");

                entity.Property(e => e.ClosingBalance).IsRequired();
                entity.HasCheckConstraint("ClosingBalance_Check", "ClosingBalance >= 0");

            });

            modelBuilder.Entity<TransactionStatus>(entity =>
            {
                entity.HasKey(e => e.TransactionStatusId).HasName("PK_Transaction_Status");

                entity.HasOne(t => t.Account)
                    .WithMany(a => a.TransactionStatuses)
                    .HasForeignKey(t => t.AccountId);

                entity.Property(e => e.Message).HasMaxLength(35);

                entity.HasCheckConstraint("SourceBalance_Check", "SourceBalance >= 0");
                entity.Property(e => e.SourceBalance).IsRequired();

            });
        }
    


    }
}
