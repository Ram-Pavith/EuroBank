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
        public DbSet<Service> Services { get; set; }


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
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "CustomerId").IsUnique();

                entity.HasIndex(e => e.EmailId, "EmailId");

                entity.Property(e => e.Firstname).HasMaxLength(50).IsUnicode(true);

                entity.Property(e => e.Lastname).HasMaxLength(50).IsUnicode(true);

                entity.Property(e=>e.Address).HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(10).IsUnicode(true);

                entity.Property(e=>e.PanNumber).HasMaxLength(15).IsUnicode(true);

                entity.Property(e => e.DOB).IsUnicode(false);

            });

            modelBuilder.Entity<CustomerCreationStatus>(entity =>
            {
                entity.HasIndex(e => e.Id, "CustomerCreationStatusId");

                entity.HasOne(e => e.Customer)
                .WithOne(p => p.CustomerCreationStatus)
                .HasForeignKey<Customer>(e => e.CustomerId);

                entity.Property(e => e.Message).HasMaxLength(100).IsUnicode(false);

            });


        }
    


    }
}
