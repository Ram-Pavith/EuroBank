﻿// <auto-generated />
using System;
using EuroBankAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EuroBankAPI.Migrations
{
    [DbContext(typeof(EuroBankContext))]
    partial class EuroBankContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EuroBankAPI.Models.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccountCreationStatusId")
                        .HasColumnType("int");

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("int");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2023, 4, 3, 15, 32, 57, 461, DateTimeKind.Local).AddTicks(1899));

                    b.HasKey("AccountId")
                        .HasName("PK_Account");

                    b.HasIndex("AccountCreationStatusId")
                        .IsUnique();

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Accounts");

                    b.HasCheckConstraint("Balance_check", "Balance >= 0");
                });

            modelBuilder.Entity("EuroBankAPI.Models.AccountCreationStatus", b =>
                {
                    b.Property<int>("AccountCreationStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountCreationStatusId"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AccountCreationStatusId")
                        .HasName("PK_Account_Creation_Status");

                    b.ToTable("AccountCreationStatuses");
                });

            modelBuilder.Entity("EuroBankAPI.Models.AccountType", b =>
                {
                    b.Property<int>("AccountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountTypeId"), 1L, 1);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.HasKey("AccountTypeId")
                        .HasName("PK_Account_Type");

                    b.ToTable("AccountTypes");
                });

            modelBuilder.Entity("EuroBankAPI.Models.CounterParty", b =>
                {
                    b.Property<string>("CounterPartyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CounterPartyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CounterPartyId");

                    b.ToTable("CounterParties");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Customer", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CustomerCreationStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DOB")
                        .IsUnicode(false)
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PanNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("CustomerId");

                    b.HasIndex("CustomerCreationStatusId")
                        .IsUnique();

                    b.HasIndex(new[] { "CustomerId" }, "CustomerId")
                        .IsUnique();

                    b.HasIndex(new[] { "EmailId" }, "EmailId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("EuroBankAPI.Models.CustomerCreationStatus", b =>
                {
                    b.Property<int>("CustomerCreationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerCreationId"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("CustomerCreationId");

                    b.HasIndex(new[] { "CustomerCreationId" }, "CustomerCreationStatusId");

                    b.ToTable("CustomerCreationStatuses");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.HasIndex(new[] { "EmailId" }, "EmailId")
                        .HasDatabaseName("EmailId1");

                    b.HasIndex(new[] { "EmployeeId" }, "EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EuroBankAPI.Models.RefPaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodCode"), 1L, 1);

                    b.Property<string>("PaymentMethodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodCode");

                    b.ToTable("RefPaymentMethods");
                });

            modelBuilder.Entity("EuroBankAPI.Models.RefTransactionStatus", b =>
                {
                    b.Property<string>("TransactionStatusCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TransactionStatusDescriptions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionStatusCode");

                    b.ToTable("RefTransactionStatuses");
                });

            modelBuilder.Entity("EuroBankAPI.Models.RefTransactionType", b =>
                {
                    b.Property<int>("TransactionTypeCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionTypeCode"), 1L, 1);

                    b.Property<string>("TransactionTypeDescriptions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionTypeCode");

                    b.ToTable("RefTransactionType");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceId"), 1L, 1);

                    b.Property<DateTime>("DateServiceProvided")
                        .HasColumnType("datetime2");

                    b.HasKey("ServiceId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Statement", b =>
                {
                    b.Property<int>("StatementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatementId"), 1L, 1);

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ClosingBalance")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Deposit")
                        .HasColumnType("float");

                    b.Property<string>("Narration")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("RefNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValueDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Withdrawal")
                        .HasColumnType("float");

                    b.HasKey("StatementId")
                        .HasName("PK_Statement_ID");

                    b.HasIndex("AccountId");

                    b.ToTable("Statements");

                    b.HasCheckConstraint("ClosingBalance_Check", "ClosingBalance >= 0");

                    b.HasCheckConstraint("Withdrawal_Check", "Withdrawal >= 0");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Transaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AmountOfTransaction")
                        .HasColumnType("float");

                    b.Property<string>("CounterPartyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfTransaction")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefTransactionStatusId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RefTransactionTypeId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("CounterPartyId");

                    b.HasIndex("RefTransactionStatusId")
                        .IsUnique();

                    b.HasIndex("RefTransactionTypeId")
                        .IsUnique();

                    b.HasIndex("ServiceId")
                        .IsUnique();

                    b.HasIndex("TransactionId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("EuroBankAPI.Models.TransactionStatus", b =>
                {
                    b.Property<int>("TransactionStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionStatusId"), 1L, 1);

                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<double>("SourceBalance")
                        .HasColumnType("float");

                    b.HasKey("TransactionStatusId")
                        .HasName("PK_Transaction_Status");

                    b.HasIndex("AccountId");

                    b.ToTable("TransactionStatuses");

                    b.HasCheckConstraint("SourceBalance_Check", "SourceBalance >= 0");
                });

            modelBuilder.Entity("EuroBankAPI.Models.UserAuth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserAuths");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Account", b =>
                {
                    b.HasOne("EuroBankAPI.Models.AccountCreationStatus", "AccountCreationStatus")
                        .WithOne("Account")
                        .HasForeignKey("EuroBankAPI.Models.Account", "AccountCreationStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Account_AccountCreationStatus");

                    b.HasOne("EuroBankAPI.Models.AccountType", "AccountType")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EuroBankAPI.Models.Customer", "Customer")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountCreationStatus");

                    b.Navigation("AccountType");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Customer", b =>
                {
                    b.HasOne("EuroBankAPI.Models.CustomerCreationStatus", "CustomerCreationStatus")
                        .WithOne("Customer")
                        .HasForeignKey("EuroBankAPI.Models.Customer", "CustomerCreationStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerCreationStatus");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Statement", b =>
                {
                    b.HasOne("EuroBankAPI.Models.Account", "Account")
                        .WithMany("Statements")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Transaction", b =>
                {
                    b.HasOne("EuroBankAPI.Models.CounterParty", "CounterParty")
                        .WithMany("Transactions")
                        .HasForeignKey("CounterPartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Transaction_CounterParty");

                    b.HasOne("EuroBankAPI.Models.RefTransactionStatus", "RefTransactionStatus")
                        .WithOne("Transaction")
                        .HasForeignKey("EuroBankAPI.Models.Transaction", "RefTransactionStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Transaction_RefTransactionStatus");

                    b.HasOne("EuroBankAPI.Models.RefTransactionType", "RefTransactionType")
                        .WithOne("Transaction")
                        .HasForeignKey("EuroBankAPI.Models.Transaction", "RefTransactionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Transaction_RefTransactionType");

                    b.HasOne("EuroBankAPI.Models.Service", "Service")
                        .WithOne("Transaction")
                        .HasForeignKey("EuroBankAPI.Models.Transaction", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Transaction_Service");

                    b.Navigation("CounterParty");

                    b.Navigation("RefTransactionStatus");

                    b.Navigation("RefTransactionType");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("EuroBankAPI.Models.TransactionStatus", b =>
                {
                    b.HasOne("EuroBankAPI.Models.Account", "Account")
                        .WithMany("TransactionStatuses")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Account", b =>
                {
                    b.Navigation("Statements");

                    b.Navigation("TransactionStatuses");
                });

            modelBuilder.Entity("EuroBankAPI.Models.AccountCreationStatus", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });

            modelBuilder.Entity("EuroBankAPI.Models.AccountType", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("EuroBankAPI.Models.CounterParty", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("EuroBankAPI.Models.Customer", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("EuroBankAPI.Models.CustomerCreationStatus", b =>
                {
                    b.Navigation("Customer")
                        .IsRequired();
                });

            modelBuilder.Entity("EuroBankAPI.Models.RefTransactionStatus", b =>
                {
                    b.Navigation("Transaction")
                        .IsRequired();
                });

            modelBuilder.Entity("EuroBankAPI.Models.RefTransactionType", b =>
                {
                    b.Navigation("Transaction")
                        .IsRequired();
                });

            modelBuilder.Entity("EuroBankAPI.Models.Service", b =>
                {
                    b.Navigation("Transaction")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
