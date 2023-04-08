using EuroBankAPI.Controllers;
using EuroBankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository;
using Org.BouncyCastle.Crypto.IO;
using System.Linq.Expressions;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Immutable;

namespace EuroBankTesting
{
    [TestFixture]
    public class AccountsControllerTest
    {
        private Mock<IUnitOfWork> _iUw;
        private Mock<IAccountRepository> accRepo;
        private Mock<IMapper> _mapper;

        private AccountsController accController;

        private Customer _customer = new Customer()
        {
            CustomerId = "CustomerEurobank",
            EmailId = "Customer@gmail.com",
            Firstname = "Customer",
            Lastname = "Eurobank",
            Phone = "4242424242",
            PasswordHash = new byte[] { 46, 174, 31, 247, 57, 63, 66, 164, 207, 113, 131, 15, 82, 113, 122, 13, 36, 124, 231, 245, 181, 92, 209, 142, 7, 222, 70, 40, 140, 162, 44, 12, 140, 20, 147, 79, 22, 23, 97, 208, 240, 158, 11, 61, 147, 12, 227, 103, 3, 229, 255, 102, 92, 145, 214, 246, 103, 146, 135, 128, 55, 28, 8, 98 },
            PasswordSalt = new byte[] { 168, 229, 206, 135, 94, 168, 32, 135, 189, 238, 81, 242, 210, 36, 152, 93, 38, 215, 53, 239, 193, 3, 107, 66, 172, 176, 29, 237, 202, 117, 7, 81, 147, 73, 195, 73, 80, 124, 84, 66, 70, 55, 197, 49, 121, 196, 83, 181, 0, 174, 75, 17, 16, 34, 56, 70, 123, 104, 86, 115, 222, 49, 208, 188, 185, 203, 90, 38, 186, 195, 45, 248, 246, 231, 73, 126, 243, 142, 13, 144, 169, 224, 192, 204, 68, 171, 198, 183, 214, 167, 87, 155, 201, 22, 15, 44, 232, 231, 85, 10, 249, 70, 75, 140, 149, 149, 89, 109, 229, 252, 46, 53, 249, 57, 168, 28, 117, 39, 92, 153, 80, 69, 115, 197, 232, 39, 135, 241 }
        };

       

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

        [SetUp]
        public void Setup()
        {
            _iUw = new Mock<IUnitOfWork>();
            accRepo = new Mock<IAccountRepository>();
            _mapper = new Mock<IMapper>();  
            accController = new AccountsController(_iUw.Object, _mapper.Object, accRepo.Object);

        }

        
        [TestCase("CustomerEurobank")]
        public void GetCustomerAccounts_Test(string Cid)
        {
            Task<Customer> cust = Task.FromResult(_customer);
            Task<IEnumerable<Account>> c_accs = Task.FromResult(_customerAccs.AsEnumerable());
            _iUw.Setup(x => x.Customers.GetAsync(x => x.CustomerId == Cid, true, null)).Returns(cust);          
            //_iUw.Setup(x => x.Accounts.GetAllAsync(x => x.CustomerId == Cid,null)).Returns(c_accs);
            var result = accController.GetCustomerAccountsBalance(Cid);
            Assert.That(result, Is.InstanceOf<Task<ActionResult<IEnumerable<AccountBalanceDTO>>>>());
        }

        //[]
    }

   
}
