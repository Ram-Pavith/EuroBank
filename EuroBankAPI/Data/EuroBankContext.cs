using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

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
                optionsBuilder.UseSqlServer("Server=DESKTOP-ETROQ1R\\SQLEXPRESS;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;");
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


                /*entity.HasOne(t => t.AccountCreationStatus)
                .WithOne(s => s.Account)
                .HasForeignKey<Account>(t => t.AccountCreationStatusId)
                .HasConstraintName("FK_Account_AccountCreationStatus");*/

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

                entity.Property(e => e.Narration).HasMaxLength(200);
                entity.HasOne(s => s.Account)
                    .WithMany(a => a.Statements)
                    .HasForeignKey(a => a.AccountId);

                entity.Property(e => e.Date).IsRequired();

                entity.Property(e => e.Narration).HasMaxLength(200).IsRequired();

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

                entity.Property(e => e.PanNumber).HasMaxLength(10).IsUnicode(true);

                entity.Property(e => e.DOB).IsUnicode(false);


                /*entity.HasOne(e => e.CustomerCreationStatus)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(e => e.CustomerCreationStatusId);*/

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

                /* entity.HasOne(t => t.Service)
                 .WithMany(s => s.Transactions)
                 .HasForeignKey(t => t.ServiceId)
                 .HasConstraintName("FK_Transaction_Service");*/

                entity.HasOne(t => t.RefTransactionType)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.RefTransactionTypeId)
                .HasConstraintName("FK_Transaction_RefTransactionType");

                entity.HasOne(t => t.RefTransactionStatus)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.RefTransactionStatusId)
                .HasConstraintName("FK_Transaction_RefTransactionStatus");

                entity.HasOne(t => t.RefPaymentMethod)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.RefPaymentMethodId)
                .HasConstraintName("FK_Transaction_RefPaymentMethod");

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

            modelBuilder.Entity<AccountCreationStatus>()
                .HasData(
                    new AccountCreationStatus() { AccountCreationStatusId = 1,Message = "Account Creation Succes" },
                    new AccountCreationStatus() { AccountCreationStatusId = 2,Message = "Account Creation Failed" }
                );
            modelBuilder.Entity<AccountType>()
                .HasData(
                    new AccountType() {AccountTypeId = 1, Type = "Savings"},
                    new AccountType() { AccountTypeId = 2,Type = "Current"}
                );
            modelBuilder.Entity<Models.Service>()
                .HasData(
                    new Models.Service() { ServiceId = 1,ServiceName = "NEFT"},
                    new Models.Service() { ServiceId = 2,ServiceName = "RTGS"},
                    new Models.Service() { ServiceId = 3,ServiceName = "IMPS"}
                );
            modelBuilder.Entity<RefTransactionStatus>()
                .HasData(
                    new RefTransactionStatus() { TransactionStatusCode = 1,TransactionStatusDescriptions = "Transaction Success" },
                    new RefTransactionStatus() { TransactionStatusCode = 2,TransactionStatusDescriptions = "Transaction Failed" },
                    new RefTransactionStatus() { TransactionStatusCode = 3,TransactionStatusDescriptions = "Withdrawal Limit Amount Exceeded" },
                    new RefTransactionStatus() { TransactionStatusCode = 4,TransactionStatusDescriptions = "Insufficient Balance" }

                );
            modelBuilder.Entity<RefTransactionType>()
                .HasData(
                    new RefTransactionType() { TransactionTypeCode = 1,TransactionTypeDescriptions = "Despost"},
                    new RefTransactionType() { TransactionTypeCode = 2,TransactionTypeDescriptions = "Withdraw"},
                    new RefTransactionType() { TransactionTypeCode = 3,TransactionTypeDescriptions = "Transfer"}
                );
            modelBuilder.Entity<RefPaymentMethod>()
                .HasData(
                    new RefPaymentMethod() { PaymentMethodCode = 1, PaymentMethodName = "Card"},
                    new RefPaymentMethod() { PaymentMethodCode = 2, PaymentMethodName = "NetBanking"}
                );
            modelBuilder.Entity<Employee>()
                .HasData(
                    new Employee()
                    {
                        EmployeeId = Guid.NewGuid(),
                        EmailId = "Employee@gmail.com",
                        Firstname = "Employee",
                        Lastname = "Eurobank",
                        PasswordHash = new byte[] { 46, 174, 31, 247, 57, 63, 66, 164, 207, 113, 131, 15, 82, 113, 122, 13, 36, 124, 231, 245, 181, 92, 209, 142, 7, 222, 70, 40, 140, 162, 44, 12, 140, 20, 147, 79, 22, 23, 97, 208, 240, 158, 11, 61, 147, 12, 227, 103, 3, 229, 255, 102, 92, 145, 214, 246, 103, 146, 135, 128, 55, 28, 8, 98 },
                        PasswordSalt = new byte[] { 168, 229, 206, 135, 94, 168, 32, 135, 189, 238, 81, 242, 210, 36, 152, 93, 38, 215, 53, 239, 193, 3, 107, 66, 172, 176, 29, 237, 202, 117, 7, 81, 147, 73, 195, 73, 80, 124, 84, 66, 70, 55, 197, 49, 121, 196, 83, 181, 0, 174, 75, 17, 16, 34, 56, 70, 123, 104, 86, 115, 222, 49, 208, 188, 185, 203, 90, 38, 186, 195, 45, 248, 246, 231, 73, 126, 243, 142, 13, 144, 169, 224, 192, 204, 68, 171, 198, 183, 214, 167, 87, 155, 201, 22, 15, 44, 232, 231, 85, 10, 249, 70, 75, 140, 149, 149, 89, 109, 229, 252, 46, 53, 249, 57, 168, 28, 117, 39, 92, 153, 80, 69, 115, 197, 232, 39, 135, 241 }
                    }
                );
            modelBuilder.Entity<CustomerCreationStatus>()
                .HasData(
                    new CustomerCreationStatus() { CustomerCreationId = 1, Message = "Customer Created Successfully"},
                    new CustomerCreationStatus() { CustomerCreationId = 2, Message = "Customer Creation Failed"}
                );
            modelBuilder.Entity<Customer>()
                .HasData(
                    new Customer()
                    {
                       DOB = DateTime.Today,
                       CustomerId = "CustomerEurobank",
                       Firstname = "Customer",
                       Lastname = "Eurobank",
                       EmailId = "Customer@gmail.com",
                       PasswordHash = new byte[] { 44, 99, 229, 133, 22, 236, 120, 175, 219, 152, 102, 76, 191, 184, 5, 210, 222, 80, 252, 24, 134, 150, 254, 124, 199, 232, 88, 65, 129, 80, 143, 236, 94, 220, 203, 124, 200, 224, 105, 183, 16, 104, 192, 211, 33, 206, 166, 253, 119, 119, 32, 175, 117, 134, 114, 84, 157, 9, 16, 202, 173, 221, 141, 74},
                       PasswordSalt = new byte[] { 225, 123, 252, 79, 109, 166, 111, 44, 27, 233, 234, 50, 0, 12, 173, 77, 152, 172, 62, 38, 219, 131, 215, 151, 221, 90, 80, 30, 226, 39, 228, 104, 40, 181, 194, 174, 237, 170, 214, 85, 222, 187, 127, 210, 134, 245, 13, 214, 99, 82, 146, 169, 226, 220, 155, 47, 202, 125, 112, 131, 93, 154, 135, 109, 127, 84, 144, 69, 242, 12, 42, 98, 229, 215, 163, 211, 136, 61, 199, 51, 217, 93, 222, 120, 128, 107, 82, 84, 229, 143, 75, 219, 143, 111, 76, 130, 199, 54, 91, 128, 211, 7, 158, 2, 218, 17, 120, 228, 219, 157, 195, 160, 5, 211, 24, 118, 193, 190, 85, 228, 103, 103, 16, 255, 218, 166, 132, 175},
                       Address = "Chennai",
                       Phone = "4242424242",
                       PanNumber = "LBKTIOPNHW",
                       CustomerCreationStatusId = 1
                    }
                );
            modelBuilder.Entity<Account>()
                .HasData(
                    new Account()
                    {
                        AccountId = Guid.NewGuid(),
                        CustomerId = "CustomerEurobank",
                        Balance = 10000,
                        AccountTypeId = 1,
                        AccountCreationStatusId = 1,
                        DateCreated = DateTime.Today
                    }
                );
        }

    } 

}


