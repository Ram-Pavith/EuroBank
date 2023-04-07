/*using AutoMapper;
using EuroBankAPI.Controllers;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EuroBankTesting
{
    public class Tests
    {
        private Mock<IUnitOfWork> unitofworkobj;
        private TransactionController TransactionController;
        private Mock<ITransactionRepository> transactionrepoobj;
        private List<Transaction> transactions=new List<Transaction>();
        private Mock<IMapper> mapper;
        private Account account=new Account();
        private Service service=new Service();
        
        [SetUp]
        public void Setup()
        {
            
            unitofworkobj=new Mock<IUnitOfWork>();
            mapper=new Mock<IMapper>();
            transactionrepoobj= new Mock<ITransactionRepository>();
            TransactionController=new TransactionController(unitofworkobj.Object,mapper.Object,transactionrepoobj.Object);
        }

*//*        [TestCase(new Guid("544F2E07-2651-45FA-8369-0C2CB6137DA4"),3500,1)]
*//*        public void WithDrawTest(Guid AccountId, double amount, int serviceId)
        {
            //account.AccountId = AccountId;
            //service.ServiceId = serviceId;
            Task<IEnumerable<Account>> c_accs = Task.FromResult(_customerAccs.AsEnumerable());
            var a = TransactionController.Withdraw(AccountId, amount, serviceId);
            Assert.That(a,Is.InstanceOf<Task<ActionResult<RefTransactionStatusDTO>>>);
        }
    }
}*/