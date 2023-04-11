using AutoMapper;
using Castle.Core.Resource;
using EuroBankAPI.DTOs;
using EuroBankAPI.Helpers;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Data.Common;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _uw;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepo;
        private readonly ILogger _logger;
        public AccountsController(IUnitOfWork uw, IMapper mapper, IAccountRepository _accRepo)
        {
            _uw = uw;
            _mapper = mapper;
            _accountRepo = _accRepo;
        }


        [HttpPost("CreateAccount")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountCreationStatusDTO>> CreateAccount(string CustomerId)
        {
            //Checking if the customer exist
            _logger.LogInformation("Create account methos is called");
            var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (CustomerExists == null)
            {
                return NotFound();
            }
            else
            {
                //Checking if the customer already has an acocunt of Type "Current"
                var AccountExists = await _uw.Accounts.GetAsync(x => x.CustomerId == CustomerId && x.AccountTypeId == 2);
                if (AccountExists != null)
                {
                    return BadRequest();
                }
                else
                {
                    //Creating a new account 
                    Account newAcc = new Account();
                    newAcc.CustomerId = CustomerId;
                    newAcc.AccountTypeId = 2;   //current account
                    newAcc.AccountCreationStatusId = 1;     // success status
                    newAcc.Balance = 10000;

                    try
                    {
                        await _uw.Accounts.CreateAsync(newAcc);
                        AccountCreationStatus acs = await _uw.AccountCreationStatuses.GetAsync(x => x.AccountCreationStatusId == newAcc.AccountCreationStatusId);
                        AccountCreationStatusDTO accCreationStatusDTO = _mapper.Map<AccountCreationStatusDTO>(acs);
                        _logger.LogInformation("Current account for the customer of Id : " + CustomerId + " is created");
                        return accCreationStatusDTO;
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
        }


        [HttpGet("GetCustomerAccountsBalance")]
        [Authorize(Roles = "Employee, Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountBalanceDTO>>> GetCustomerAccountsBalance(string CustomerId)
        {
            //Checking if the customer exist
            _logger.LogInformation("Account balance of the customer " + CustomerId + "is called");
            var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (CustomerExists == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<Account> customerAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                //IEnumerable<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<IEnumerable<AccountBalanceDTO>>(customerAccounts); --error
                List<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<List<AccountBalanceDTO>>(customerAccounts);
                _logger.LogInformation("Account balance of the customer " + CustomerId + "is got");
                return customerAccountsDTO;
            }
        }

        [HttpGet("GetAccountBalance")]
        [Authorize(Roles = "Employee, Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountBalanceDTO>> GetAccountBalance(Guid AccountId)
        {
            //Checking if account exist
            _logger.LogInformation("Account balance of Account Id: " + AccountId + "is called");
            Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (targetAccount == null)
            {
                return NotFound();
            }
            else
            {
                var targetAccountDTO = _mapper.Map<AccountBalanceDTO>(targetAccount);
                _logger.LogInformation("Account balance of Account Id: " + AccountId + "is got");

                return targetAccountDTO;
            }
        }

        [HttpGet("GetAccountStatement")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StatementDTO>>> GetAccountStatement(Guid AccountId, DateTime? from_date, DateTime? to_date)
        {
            //Checking if account exist
            _logger.LogInformation("GetAccountStatement method is called");
            Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (targetAccount == null)
            {
                return NotFound();
            }
            else
            {
                if (from_date != null && to_date != null)
                {
                    IEnumerable<Statement> stmt = await _uw.Statements.GetAllAsync(x => x.AccountId == AccountId &&
                                                                        x.Date >= from_date && x.Date <= to_date);

                    List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                    return AccountStatement;
                }
                else
                {
                    IEnumerable<Statement> stmt = await _uw.Statements.GetAllAsync(x => x.AccountId == AccountId &&
                                                                        x.Date.Month == DateTime.Now.Month);
                    List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                    _logger.LogInformation("CustomerStatement of " + AccountId + " from " + from_date + "to" + to_date);
                    return AccountStatement;
                }
            }

        }
        [HttpGet("GetTransactions")]
        [Authorize(Roles = "Employee,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions(Guid  AccountId, int PageSize = 0, int PageNumber = 1)
        {
            _logger.LogInformation("Get transactions method is called");
            var account = await _uw.Accounts.GetAllAsync(x => x.AccountId == AccountId);
            if (account == null)
            {
                return BadRequest("Account does not exist");
            }
            IEnumerable<Transaction> transactions;
            if (PageSize <= 0)
            {
                transactions = await _uw.Transactions.GetAllAsync(x => x.AccountId == AccountId);
            }
            else
            {
                transactions = await _uw.Transactions.GetAllAsync(x => x.AccountId == AccountId, pageSize: PageSize, pageNumber: PageNumber);
            }
            List<TransactionDTO> transactionsDTO = _mapper.Map<List<TransactionDTO>>(transactions);
            _logger.LogInformation("GetTransactions of the Customer of AccountId: " + AccountId + " is got successfully");
            return transactionsDTO;
            
        }

        [HttpGet("AccountPages")]
        [Authorize(Roles = "Employee,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AccountPages(int pageSize)
        {
            decimal count = await _uw.Accounts.CountAsync();
            return Convert.ToInt32(Math.Ceiling((decimal)(count / pageSize)));
        }

        [HttpGet("AccountTransactionPages")]
        [Authorize(Roles = "Employee,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AccountTransactionPages(int pageSize,Guid accountId)
        {
            var accountTransactions = await _uw.Transactions.GetAllAsync(x => x.AccountId == accountId);
            decimal count = accountTransactions.Count();
            return Convert.ToInt32(Math.Ceiling((decimal)(count / pageSize)));
        }



    }
}
