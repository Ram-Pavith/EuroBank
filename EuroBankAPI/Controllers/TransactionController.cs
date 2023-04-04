using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IUnitOfWork _uw;
        private readonly ILogger<TransactionController> _logger;
        private readonly IMapper _mapper;

        public TransactionController(IUnitOfWork uw, IMapper mapper, ILogger<TransactionController> logger)
        {
            _uw =uw ;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("withdraw")]
        [Authorize(Roles ="Customer")]
        public async Task<ActionResult<RefTransactionStatusDTO>> Withdraw(Guid AccountId,double amount,Account inacc )
        {
            var AccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if(AccountExists == null )
            {
                return BadRequest("account does not exists");
            }
            else 
            {
                try
                {
                    Transaction Transaction = new();
                    Account newacc = new();
                    //check for rule microservice
                    if (inacc.Balance > amount)
                    {
                        newacc.Balance -= inacc.Balance;
                        await _uw.Accounts.UpdateAsync(newacc);
                        Transaction.RefTransactionStatusId = "success";
                    }
                    else
                    {
                        Transaction.RefTransactionStatusId = "failure";
                    }
                    RefTransactionStatus obj = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
                    RefTransactionStatusDTO objDTO = _mapper.Map<RefTransactionStatusDTO>(obj);
                    return objDTO;
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (SqlException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                
            }                  
        }
        [HttpPost]
        [Route("deposit")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RefTransactionStatusDTO>> Deposit(Guid AccountId, double amount, Account inacc)
        {
            var AccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (AccountExists == null)
            {
                return BadRequest("account does not exists");
            }
            else
            {
                try
                {
                    Transaction Transaction = new();
                    Account newacc = new();
                    newacc.AccountId = AccountId;
                    newacc.Balance += inacc.Balance;
                    await _uw.Accounts.UpdateAsync(newacc);
                    Transaction.RefTransactionStatusId = "success";
                    RefTransactionStatus obj = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
                    RefTransactionStatusDTO objDTO = _mapper.Map<RefTransactionStatusDTO>(obj);
                    return objDTO;
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (SqlException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpPost]
        [Route("transfer")]
        [Authorize(Roles = "Customer")]

        public async Task<ActionResult<RefTransactionStatusDTO>> Transfer(Guid Source_AccountId,Guid Target_AccountId, double amount, Account inacc)
        {
            var SourceAccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == Source_AccountId);
            var TargetAccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == Target_AccountId);
            if(SourceAccountExists == null)
            {
                return NotFound();
            }
            else if(TargetAccountExists == null)
            {
                return BadRequest("Target Account does not exists");
            }
            else
            {
                try
                {
                    Transaction Transaction = new();
                    Account sourceacc = new();
                    Account targetacc = new();
                    //sourceacc.AccountId = Source_AccountId;
                    //targetacc.AccountId = Target_AccountId;
                    if (sourceacc.Balance > amount)
                    {
                        sourceacc.Balance -= amount;
                        targetacc.Balance += amount;
                        await _uw.Accounts.UpdateAsync(sourceacc);
                        await _uw.Accounts.UpdateAsync(targetacc);
                        Transaction.RefTransactionStatusId = "success";
                    }
                    else
                    {
                        Transaction.RefTransactionStatusId = "failure";
                    }
                    RefTransactionStatus obj = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
                    RefTransactionStatusDTO objDTO = _mapper.Map<RefTransactionStatusDTO>(obj);
                    return objDTO;
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (SqlException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }
        [HttpGet]
        [Authorize(Roles ="Employee")]
        public async Task<ActionResult<TransactionDTO>> GetTransactions(string CustomerId)
        {
            Customer CustomerIdObj = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if(CustomerIdObj == null)
            {
                return BadRequest("Customer does not exist");
            }
            else
            {
                TransactionDTO transactionDTO = _mapper.Map<TransactionDTO>(CustomerIdObj);
                return transactionDTO;
            }
        }
    }
}
