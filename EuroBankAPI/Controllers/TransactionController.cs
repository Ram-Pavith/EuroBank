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
            _uw = uw;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Withdraw")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefTransactionStatusDTO>> Withdraw(Guid AccountId, double amount, int paymentId)
        {
            _logger.LogInformation("Withdraw in AccountId " + AccountId + " is done");

            if (amount < 0)
            {
                return BadRequest("Amount to withdraw should a positive value, did you mean to Deposit amount?");
            }
            var AccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (AccountExists == null)
            {
                return BadRequest("Account does not exists");
            }
            else
            {
                try
                {

                    Transaction transaction = new();
                    //check for rule microservice

                    if (AccountExists.Balance > amount && amount < 50000)
                    {
                        AccountExists.Balance -= amount;
                        await _uw.Accounts.UpdateAsync(AccountExists);
                        transaction.RefTransactionStatusId = 1;
                    }
                    if (AccountExists.Balance < amount)
                    {
                        transaction.RefTransactionStatusId = 4;
                        var refTransactionStatusError = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == transaction.RefTransactionStatusId);
                        var refTransactionStatusErrorDTO = _mapper.Map<RefTransactionStatusDTO>(refTransactionStatusError);
                    }
                    if (amount > 50000)
                    {
                        transaction.RefTransactionStatusId = 3;
                        var refTransactionStatusError = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == transaction.RefTransactionStatusId);
                        var refTransactionStatusErrorDTO = _mapper.Map<RefTransactionStatusDTO>(refTransactionStatusError);
                    }
                    if (AccountExists.Balance < amount)
                    {
                        transaction.RefTransactionStatusId = 4;
                        var refTransactionStatusError = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == transaction.RefTransactionStatusId);
                        var refTransactionStatusErrorDTO = _mapper.Map<RefTransactionStatusDTO>(refTransactionStatusError);
                        //return refTransactionStatusErrorDTO;
                    }

                    CounterParty counterPartyExists = await _uw.CounterParties.GetAsync(x => x.CounterPartyId == AccountExists.AccountId);
                    if (counterPartyExists == null)
                    {
                        counterPartyExists = new CounterParty();
                        counterPartyExists.CounterPartyId = AccountExists.AccountId;
                        counterPartyExists.CounterPartyName = AccountExists.CustomerId;
                        await _uw.CounterParties.CreateAsync(counterPartyExists);
                    }
                    //transaction initialising
                    transaction.CounterPartyId = counterPartyExists.CounterPartyId;
                    transaction.AccountId = AccountExists.AccountId;
                    transaction.ServiceId = 1;
                    transaction.RefTransactionTypeId = 1;
                    transaction.DateOfTransaction = DateTime.Now;
                    transaction.AmountOfTransaction = amount;
                    transaction.RefPaymentMethodId = paymentId;
                    await _uw.Transactions.CreateAsync(transaction);
                    //statement inialising
                    var statement = new Statement();
                    statement.AccountId = AccountExists.AccountId;
                    statement.Date = DateTime.Today;
                    var service = await _uw.RefPaymentMethods.GetAsync(x => x.PaymentMethodCode == paymentId);
                    statement.Narration = "Deposit using  of " + amount.ToString() + " Rupees To " + AccountExists.AccountId.ToString();
                    statement.RefNo = "Deposit of " + amount.ToString() + " from " + AccountExists.AccountId.ToString();
                    statement.Deposit = amount;
                    statement.Withdrawal = 0;
                    statement.ValueDate = DateTime.Today;
                    statement.ClosingBalance = AccountExists.Balance;
                    await _uw.Statements.CreateAsync(statement);
                    var refTransactionStatus = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == transaction.RefTransactionStatusId);
                    var refTransactionStatusDTO = _mapper.Map<RefTransactionStatusDTO>(refTransactionStatus);
                    return refTransactionStatusDTO;
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }


            }
        }
        [HttpGet("Deposit")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefTransactionStatusDTO>> Deposit(Guid AccountId, double amount, int paymentId)
        {
            _logger.LogInformation("Deposit to AccountId " + AccountId + " of amount" + amount + "is done");
            if (amount < 0)
            {
                return BadRequest("Amount to Deposit should be a positive value, did you mean to Withdraw amount?");
            }
            var AccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (AccountExists == null)
            {
                return BadRequest("Account does not exists");
            }
            else
            {
                try
                {
                    Transaction transaction = new();
                    AccountExists.Balance += amount;
                    await _uw.Accounts.UpdateAsync(AccountExists);
                    CounterParty counterPartyExists = await _uw.CounterParties.GetAsync(x => x.CounterPartyId == AccountExists.AccountId);
                    if (counterPartyExists == null)
                    {
                        counterPartyExists = new CounterParty();
                        counterPartyExists.CounterPartyId = AccountExists.AccountId;
                        counterPartyExists.CounterPartyName = AccountExists.CustomerId;
                        await _uw.CounterParties.CreateAsync(counterPartyExists);
                    }
                    if (amount < 200000)
                    {
                        transaction.RefTransactionStatusId = 1;
                    }
                    if (AccountExists.Balance > amount)
                    {
                        transaction.RefTransactionStatusId = 1;
                    }
                    if (amount > 200000)
                    {
                        transaction.RefTransactionStatusId = 2;
                    }

                    //transaction initialising
                    transaction.CounterPartyId = counterPartyExists.CounterPartyId;
                    transaction.AccountId = AccountExists.AccountId;
                    transaction.ServiceId = 1;
                    transaction.RefTransactionTypeId = 2;
                    transaction.DateOfTransaction = DateTime.Now;
                    transaction.AmountOfTransaction = amount;
                    transaction.RefPaymentMethodId = paymentId;
                    await _uw.Transactions.CreateAsync(transaction);
                    var service = await _uw.RefPaymentMethods.GetAsync(x => x.PaymentMethodCode == paymentId);
                    //statement inialising
                    var statement = new Statement();
                    statement.AccountId = AccountExists.AccountId;
                    statement.Date = DateTime.Today;
                    statement.Narration = "withdrawal of " + amount.ToString() + " Rupees To " + AccountExists.AccountId.ToString();
                    statement.RefNo = "Withdrawal of " + amount.ToString() + " from " + AccountExists.AccountId.ToString();
                    statement.Deposit = 0;
                    statement.Withdrawal = amount;
                    statement.ValueDate = DateTime.Today;
                    statement.ClosingBalance = AccountExists.Balance;
                    await _uw.Statements.CreateAsync(statement);
                    var refTransactionStatus = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == transaction.RefTransactionStatusId);
                    var refTransactionStatusDTO = _mapper.Map<RefTransactionStatusDTO>(refTransactionStatus);

                    return refTransactionStatusDTO;
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpGet("Transfer")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RefTransactionStatusDTO>> Transfer(Guid Source_AccountId, Guid Target_AccountId, double amount, int serviceId)
        {
            if (amount < 0)
            {
                return BadRequest("Cannot Transfer an amount of negative value, please enter positive amount value");
            }
            var SourceAccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == Source_AccountId);
            var TargetAccountExists = await _uw.Accounts.GetAsync(x => x.AccountId == Target_AccountId);
            if (SourceAccountExists == null)
            {
                return NotFound();
            }
            else if (TargetAccountExists == null)
            {
                return BadRequest("Target Account does not exists");
            }
            else
            {
                try
                {
                    Transaction transaction = new();
                    if (SourceAccountExists.Balance > amount)
                    {
                        SourceAccountExists.Balance -= amount;
                        TargetAccountExists.Balance += amount;
                        await _uw.Accounts.UpdateAsync(SourceAccountExists);
                        await _uw.Accounts.UpdateAsync(TargetAccountExists);
                        transaction.RefTransactionStatusId = 1;
                    }
                    if (SourceAccountExists.Balance < amount)
                    {
                        transaction.RefTransactionStatusId = 4;
                    }

                    CounterParty counterPartyExists = await _uw.CounterParties.GetAsync(x => x.CounterPartyId == TargetAccountExists.AccountId);
                    if (counterPartyExists == null)
                    {
                        counterPartyExists = new CounterParty();
                        counterPartyExists.CounterPartyId = TargetAccountExists.AccountId;
                        counterPartyExists.CounterPartyName = TargetAccountExists.CustomerId;
                        await _uw.CounterParties.CreateAsync(counterPartyExists);
                    }
                    //transaction initialising
                    transaction.CounterPartyId = counterPartyExists.CounterPartyId;
                    transaction.AccountId = SourceAccountExists.AccountId;
                    transaction.ServiceId = 3;
                    transaction.RefTransactionTypeId = 3;
                    transaction.DateOfTransaction = DateTime.Now;
                    transaction.AmountOfTransaction = amount;
                    transaction.RefPaymentMethodId = 2;
                    await _uw.Transactions.CreateAsync(transaction);
                    var service = await _uw.Services.GetAsync(x => x.ServiceId == serviceId);
                    //statement initialising
                    //statement inialising
                    var statement = new Statement();
                    statement.AccountId = SourceAccountExists.AccountId;
                    statement.Date = DateTime.Today;
                    statement.Narration = "Transfer using " + service.ServiceName.ToString() + " of " + amount.ToString() + " Rupees From " + SourceAccountExists.AccountId.ToString() + " To " + TargetAccountExists.AccountId.ToString();
                    statement.RefNo = "Transfer of " + amount.ToString() + " To " + TargetAccountExists.AccountId.ToString();
                    statement.Deposit = amount;
                    statement.Withdrawal = 0;
                    statement.ValueDate = DateTime.Today;
                    statement.ClosingBalance = SourceAccountExists.Balance;
                    await _uw.Statements.CreateAsync(statement);
                    var refTransactionStatus = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == transaction.RefTransactionStatusId);
                    var refTransactionStatusDTO = _mapper.Map<RefTransactionStatusDTO>(refTransactionStatus);
                    //RefTransactionStatus obj = await _uw.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
                    //RefTransactionStatusDTO objDTO = _mapper.Map<RefTransactionStatusDTO>(obj);
                    return refTransactionStatusDTO;
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
       
        [HttpGet("GetTransactionById")]
        [Authorize(Roles = "Employee,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(Guid TransactionId)
        {
            var transaction = await _uw.Transactions.GetAsync(x => x.TransactionId == TransactionId);
            if(transaction == null)
            {
                return BadRequest("Transaction Id does not exist");
            }
            TransactionDTO transactionDTO = _mapper.Map<TransactionDTO>(transaction);
            return Ok(transactionDTO);
        }
    }
}
