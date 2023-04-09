using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceChargesWorker
{
    public class AppDbContext : DbContext, IServiceProvider
    {

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base() { }
        protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-LJOJLTJ\\SQLEXPRESS;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountCreationStatus> AccountCreationStatuses { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
