using AutoMapper;
using Azure;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Data;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CustomerController(IUnitOfWork context, ILogger<CustomerController> logger, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("CustomerLogin")]
        public async Task<ActionResult<UserAuthResponseDTO>> CustomerLogin(CustomerLoginDTO customerLogin)
        {
            UserAuthResponseDTO response;
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(customerLogin);
                response = await _authService.LoginEmployeeAndCustomer(request);
                if (response.Success)
                    return Ok(response);
                else
                    return BadRequest(response.Message);
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

        [HttpGet("GetCustomerAccounts")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
       
        public async Task<ActionResult<IEnumerable<AccountDTO>>> getCustomerAccounts(string CustomerId)
        {
            try
            {
                var CustomerExists = await _context.Customers.GetAsync(x => x.CustomerId == CustomerId);
                if (CustomerExists == null)
                {
                    return NotFound();
                }
                else
                {
                    IEnumerable<Account> customerAccounts = await _context.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                    List<AccountDTO> AccountsDTO = _mapper.Map<List<AccountDTO>>(customerAccounts);
                    return AccountsDTO;
                }
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

        [HttpGet("GetAccount")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid AccountId)
        {
            try
            {
                //Checking is account exist
                Account targetAccount = await _context.Accounts.GetAsync(x => x.AccountId == AccountId);
                if (targetAccount == null)
                {
                    return NotFound();
                }
                else
                {
                    AccountDTO targetAccountDTO = _mapper.Map<AccountDTO>(targetAccount);
                    return targetAccountDTO;
                }
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

        [HttpGet("GetAccountStatement")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StatementDTO>>> GetCustomerStatement(string CustomerId, DateTime? from_date, DateTime? to_date)
        {
            try
            {
                List<Statement> statementCustomer = new List<Statement>();

                //Checking is account exist
                var targetAccounts = await _context.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                if (targetAccounts == null)
                {
                    return NotFound();
                }
                foreach (var account in targetAccounts)
                {
                    
                    if (from_date != null && to_date != null)
                    {
                        IEnumerable<Statement> stmt = await _context.Statements.GetAllAsync(x => x.AccountId == account.AccountId &&
                                                                            x.Date >= from_date && x.Date <= to_date);
                        List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                        statementCustomer.AddRange(stmt);
                    }
                    else
                    {
                        IEnumerable<Statement> stmt = await _context.Statements.GetAllAsync(x => x.AccountId == account.AccountId &&
                                                                            x.Date.Month == DateTime.Now.Month);
                        List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                        statementCustomer.AddRange(stmt);
                    }
                }
                var statementCustomerDTO = _mapper.Map<List<StatementDTO>>(statementCustomer);
                return statementCustomerDTO;

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
        [HttpGet("ViewAllTransactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> ViewAllTransaction()
        {
            try
            {
                var Transactions = await _context.Transactions.GetAllAsync();

                List<TransactionDTO> TransactionDTOs = _mapper.Map<List<TransactionDTO>>(Transactions);

                return TransactionDTOs;
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
       /* [HttpPost]
        [Route("withdraw")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RefTransactionStatusDTO>> Withdraw(Guid AccountId, double amount, Account inacc)
        {
            var AccountExists = await _context.Accounts.GetAsync(x => x.AccountId == AccountId);
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
                    //check for rule microservice
                    if (inacc.Balance > amount)
                    {
                        newacc.Balance -= inacc.Balance;
                        await _context.Accounts.UpdateAsync(newacc);
                        Transaction.RefTransactionStatusId = "success";
                    }
                    else
                    {
                        Transaction.RefTransactionStatusId = "failure";
                    }
                    RefTransactionStatus obj = await _context.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
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
            var AccountExists = await _context.Accounts.GetAsync(x => x.AccountId == AccountId);
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
                    await _context.Accounts.UpdateAsync(newacc);
                    Transaction.RefTransactionStatusId = "success";
                    RefTransactionStatus obj = await _context.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
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

        public async Task<ActionResult<RefTransactionStatusDTO>> Transfer(Guid Source_AccountId, Guid Target_AccountId, double amount, Account inacc)
        {
            var SourceAccountExists = await _context.Accounts.GetAsync(x => x.AccountId == Source_AccountId);
            var TargetAccountExists = await _context.Accounts.GetAsync(x => x.AccountId == Target_AccountId);
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
                    Transaction Transaction = new();
                    Account sourceacc = new();
                    Account targetacc = new();
                    //sourceacc.AccountId = Source_AccountId;
                    //targetacc.AccountId = Target_AccountId;
                    if (sourceacc.Balance > amount)
                    {
                        sourceacc.Balance -= amount;
                        targetacc.Balance += amount;
                        await _context.Accounts.UpdateAsync(sourceacc);
                        await _context.Accounts.UpdateAsync(targetacc);
                        Transaction.RefTransactionStatusId = "success";
                    }
                    else
                    {
                        Transaction.RefTransactionStatusId = "failure";
                    }
                    RefTransactionStatus obj = await _context.RefTransactionStatuses.GetAsync(x => x.TransactionStatusCode == Transaction.RefTransactionStatusId);
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
        }*/
        [HttpPut("ResetPassword")]
        public async Task<ActionResult<CustomerDTO>> ResetPassword(string Email, string Password)
        {
            try
            {
                Customer customer = await _context.Customers.GetAsync(x => x.EmailId == Email);
                _authService.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);

                customer.PasswordHash = passwordHash;
                customer.PasswordSalt = passwordSalt;
                await _context.Customers.UpdateAsync(customer);

                CustomerDTO customerDTO = _mapper.Map<CustomerDTO>(customer);
                return customerDTO;

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
}
