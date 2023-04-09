using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroBankTesting
{
    public class AccountsRepositoryTest
    {
        private List<Account> Accounts = new List<Account>();
        IQueryable<Account> AccountData;
        Mock<DbSet<Account>> mockSet;
        Mock<EuroBankContext> mockAPIContext;
        AccountRepository AccountRepository;


        [SetUp]
        public void Setup()
        {
            Accounts = new List<Account>() {
                new Account
            {
                AccountId = Guid.NewGuid(),
                CustomerId = "CustomerEurobank",
                Balance = 10000,
                AccountTypeId = 1,
                AccountCreationStatusId = 1,
                DateCreated = DateTime.Today
            },
            new Account
            {
                AccountId = Guid.NewGuid(),
                CustomerId = "CustomerEurobank",
                Balance = 10000,
                AccountTypeId = 1,
                AccountCreationStatusId = 1,
                DateCreated = DateTime.Today
            },
            new Account
            {
                AccountId = Guid.NewGuid(),
                CustomerId = "CustomerEurobank",
                Balance = 10000,
                AccountTypeId = 1,
                AccountCreationStatusId = 1,
                DateCreated = DateTime.Today
            }
            };
            AccountData = Accounts.AsQueryable();
            mockSet = new Mock<DbSet<Account>>();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(AccountData.Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(AccountData.Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(AccountData.ElementType);
            mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(AccountData.GetEnumerator());
            var p = new DbContextOptions<EuroBankContext>();
            mockAPIContext = new Mock<EuroBankContext>(p);
            mockAPIContext.Setup(x => x.Accounts).Returns(mockSet.Object);
            AccountRepository = new AccountRepository(mockAPIContext.Object);

        }


        [Test]
        public async Task GetAsync_Test()
        {
            var AccountResult = AccountRepository.GetAsync(x=> x.CustomerId == "CustomerEurobank");
            var result = await AccountResult;
            double amount = result.Balance;
            Assert.AreEqual(10000, amount);
        }


    }
}
