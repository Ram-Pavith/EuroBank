using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroBankTesting
{
    [TestFixture]
    internal class AccountsRepoTest
    {
        private List<Account> _customerAccs = new List<Account>()
        {
            new Account
            {
                AccountId = Guid.Parse("8090600D-6EC6-4BFB-8D36-DDF959ABABE8"),
                AccountTypeId = 1,
                AccountCreationStatusId = 1,
                CustomerId = "CustomerEurobank",
                DateCreated = DateTime.Parse("07-07-2023"),
                Balance = 10000
            },
            new Account
            {
                AccountId = Guid.Parse("E070F9B8-2522-4CDB-8709-26EC876CB2E7"),
                AccountTypeId = 2,
                AccountCreationStatusId = 1,
                CustomerId = "CustomerEurobank",
                DateCreated = DateTime.Parse("07-08-2023"),
                Balance = 20000
            }
        };

        //List<Account> _customerAccountss;

        IQueryable<Account> _customerAccounts;
        Mock<DbSet<Account>> mockSet;
        Mock<EuroBankContext> mockAPIContext;
        AccountRepository accRepo;
        Mock<GenericRepository<Account>> _genRepo;
        [SetUp]
        public void Setup()
        {
            _customerAccounts = _customerAccs.AsQueryable();
            //var _customerAccounts = _customerAccountss.AsQueryable().BuildMockDbSet();
            mockSet = new Mock<DbSet<Account>>();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(_customerAccounts.Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(_customerAccounts.Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(_customerAccounts.ElementType);
            mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(_customerAccounts.GetEnumerator());
            var p = new DbContextOptions<EuroBankContext>();
            mockAPIContext = new Mock<EuroBankContext>(p);
            mockAPIContext.Setup(x => x.Set<Account>()).Returns(mockSet.Object);
            _genRepo = new Mock<GenericRepository<Account>>(mockAPIContext.Object);
            accRepo = new AccountRepository(mockAPIContext.Object);
        }

        [TestCase("8090600D-6EC6-4BFB-8D36-DDF959ABABE8")]
        public void GetAccBalance_Test(string Aid)
        {

            Guid accId = Guid.Parse(Aid);
            var result = accRepo.GetAsync(x => x.AccountId == accId);
            System.Diagnostics.Debug.WriteLine(result);
            System.Diagnostics.Debug.WriteLine(result.Result.AccountType);
            System.Diagnostics.Debug.WriteLine(result.Result);      
            double balance = result.Result.Balance;
            Assert.That(balance, Is.EqualTo(10000));
        }
    }
}
