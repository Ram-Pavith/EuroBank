using AutoMapper;
using EuroBankAPI.Controllers;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EuroBankTest
{
    public class Tests
    {
        private TransactionController _transactionController;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ILogger<TransactionController>> _logger;
        private Mock<IMapper> _mapper;
        
        private Transaction transaction;
        private Account account;
        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<TransactionController>>();
            _mapper = new Mock<IMapper>();
            _transactionController = new TransactionController(_unitOfWork.Object, _mapper.Object, _logger.Object);

            transaction=new Transaction();
            account = new Account();



        }
        
        

        [TestCase("544f2e07-2651-45fa-8369-0c2cb6137da4")]
        public void GetTransaction(Guid transactionId)
        {
            Task<Transaction> transactions = Task.FromResult(transaction);
            object value = _unitOfWork.Setup(p => p.Transactions.GetAsync(x=>x.TransactionId==transactionId, true, null)).Returns(transactions);
            var result = _transactionController.GetTransactionById(transactionId);
            Assert.That(result, Is.InstanceOf<Task<ActionResult<TransactionDTO>>>());
        }

        [TestCase("544f2e07-2651-45fa-8369-0c2cb6137da4", 5000,1)]
        public void Withdraw(Guid AccountId,double amount,int ServiceId)
        {
            Task<Account> accounts = Task.FromResult(account);
            object value = _unitOfWork.Setup(p => p.Accounts.GetAsync(x => x.AccountId == AccountId, true, null)).Returns(accounts);
            var result = _transactionController.Withdraw(AccountId,amount,ServiceId);
            Assert.That(result, Is.InstanceOf<Task<ActionResult<RefTransactionStatusDTO>>>());
        }
    }
}
