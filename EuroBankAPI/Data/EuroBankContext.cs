using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Data
{
    public class EuroBankContext : DbContext
    {
        public EuroBankContext() { }
        public EuroBankContext(DbContextOptions<EuroBankContext> options) : base(options) { }

        //DbSet Tables For the Context
        public DbSet<UserAuth> UserAuths { get; set; }

        //Context Configuring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-LJOJLTJ\\SQLEXPRESS;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
         //Accounts Microservice Entities
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountCreationStatus> AccountCreationStatuses { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCreationStatus> CustomerCreationStatuses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
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

                entity.HasOne(t => t.AccountCreationStatus)
                .WithOne(s => s.Account)
                .HasForeignKey<Account>(t => t.AccountCreationStatusId)
                .HasConstraintName("FK_Account_AccountCreationStatus");

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

                entity.Property(e => e.Message).HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Statement>(entity =>
            {
                entity.HasKey(e => e.StatementId).HasName("PK_Statement_ID");

                entity.Property(e => e.Narration).HasMaxLength(35);
                entity.HasOne(s => s.Account)
                    .WithMany(a => a.Statements)
                    .HasForeignKey(a => a.AccountId);

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


            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "CustomerId").IsUnique();

                entity.HasIndex(e => e.EmailId, "EmailId");

                entity.Property(e => e.Firstname).HasMaxLength(50).IsUnicode(true);

                entity.Property(e => e.Lastname).HasMaxLength(50).IsUnicode(true);

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(10).IsUnicode(true);

                entity.Property(e => e.PanNumber).HasMaxLength(15).IsUnicode(true);

                entity.Property(e => e.DOB).IsUnicode(false);

                entity.HasOne(e => e.CustomerCreationStatus)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(e => e.CustomerCreationStatusId);

            });

            modelBuilder.Entity<CustomerCreationStatus>(entity =>
            {
                entity.HasIndex(e => e.CustomerCreationId, "CustomerCreationStatusId");

                entity.Property(e => e.Message).HasMaxLength(100).IsUnicode(false);

            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId, "EmployeeId");

                entity.HasIndex(e => e.EmailId, "EmailId");

                entity.Property(e => e.EmailId).IsRequired();

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(50);
            });


            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.HasIndex(e => e.TransactionId);

                entity.HasOne(e => e.CounterParty)
                .WithMany(c => c.Transactions)
                .HasForeignKey(c => c.CounterPartyId)
                .HasConstraintName("FK_Transaction_CounterParty");

                entity.HasOne(t => t.Service)
                .WithOne(s => s.Transaction)
                .HasForeignKey<Transaction>(t => t.ServiceId)
                .HasConstraintName("FK_Transaction_Service");

                entity.HasOne(t => t.RefTransactionType)
                .WithOne(s => s.Transaction)
                .HasForeignKey<Transaction>(t => t.RefTransactionTypeId)
                .HasConstraintName("FK_Transaction_RefTransactionType");

                entity.HasOne(t => t.RefTransactionStatus)
                .WithOne(s => s.Transaction)
                .HasForeignKey<Transaction>(t => t.RefTransactionStatusId)
                .HasConstraintName("FK_Transaction_RefTransactionStatus");

            });
            modelBuilder.Entity<RefTransactionStatus>(entity =>
            {
                entity.HasKey(t => t.TransactionStatusCode);

            });
            modelBuilder.Entity<RefTransactionType>(entity =>
            {
                entity.HasKey(t => t.TransactionTypeCode);
            });
            modelBuilder.Entity<CounterParty>(entity =>
            {
                entity.HasKey(t => t.CounterPartyId);
            });
            modelBuilder.Entity<Models.Service>(entity =>
            {
                entity.HasKey(t => t.ServiceId);

            });
        }


    } 

}


