using AutoMapper;
using EuroBankAPI.Controllers;
using EuroBankAPI.Models;
using EuroBankAPI.Repository;
using EuroBankAPI.Repository.IRepository;
using Microsoft.Extensions.Logging;
using Moq;

namespace EuroBankTesting
{
    public class Tests
    {
        private Mock<IUnitOfWork> unitofworkobj;
        private TransactionController TransactionController;
        private Mock<ILogger<TransactionController>> transactionrepoobj;
        private List<Transaction> transactions=new List<Transaction>();
        private Mock<IMapper> mapper;


        [SetUp]
        public void Setup()
        {
            unitofworkobj=new Mock<IUnitOfWork>();
            mapper=new Mock<IMapper>();
            transactionrepoobj= new Mock<ILogger<TransactionController>>();
            TransactionController=new TransactionController(unitofworkobj.Object,mapper.Object,transactionrepoobj.Object);
        }

        [Test]
        public void Withdraw()
        {
            Assert.Pass();
        }
    }
}