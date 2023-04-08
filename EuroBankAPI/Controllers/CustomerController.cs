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
        private readonly IUnitOfWork _uw;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CustomerController(IUnitOfWork uw, ILogger<CustomerController> logger, IMapper mapper, IAuthService authService)
        {
            _uw = uw;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("CustomerLogin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> getCustomerAccounts(string CustomerId)
        {
            try
            {
                var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
                if (CustomerExists == null)
                {
                    return NotFound();
                }
                else
                {
                    IEnumerable<Account> customerAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid AccountId)
        {
            try
            {
                //Checking is account exist
                Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StatementDTO>>> GetCustomerStatement(string CustomerId, DateTime? from_date, DateTime? to_date)
        {
            try
            {
                List<Statement> statementCustomer = new List<Statement>();

                //Checking is account exist
                var targetAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                if (targetAccounts == null)
                {
                    return NotFound();
                }
                foreach (var account in targetAccounts)
                {
                    
                    if (from_date != null && to_date != null)
                    {
                        IEnumerable<Statement> stmt = await _uw.Statements.GetAllAsync(x => x.AccountId == account.AccountId &&
                                                                            x.Date >= from_date && x.Date <= to_date);
                        List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                        statementCustomer.AddRange(stmt);
                    }
                    else
                    {
                        IEnumerable<Statement> stmt = await _uw.Statements.GetAllAsync(x => x.AccountId == account.AccountId &&
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
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> ViewAllTransaction(string CustomerId)
        {
            var targetAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
            if (targetAccounts == null)
            {
                return BadRequest("Customer not found");
            }
            List<Transaction> Transactions = new ();
            IEnumerable<Transaction> TransactionAccounts;
            try
            {
                foreach(var account in targetAccounts) {
                    TransactionAccounts = await _uw.Transactions.GetAllAsync(x => x.AccountId == account.AccountId);
                    Transactions.AddRange(TransactionAccounts);
                }

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


        [HttpPut("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDTO>> ResetPassword(string Email, string Password)
        {
            try
            {
                Customer customer = await _uw.Customers.GetAsync(x => x.EmailId == Email);
                _authService.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);

                customer.PasswordHash = passwordHash;
                customer.PasswordSalt = passwordSalt;
                await _uw.Customers.UpdateAsync(customer);
                _uw.Save();
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
        [HttpDelete("RemoveCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> DeleteCustomer(string CustomerId)
        {
            var customerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (customerExists == null)
            {
                return BadRequest("Employee With the EmployeeId does not Exists");
            }
            await _uw.Customers.DeleteAsync(customerExists);
            return Ok(customerExists);
        }
    }
}
