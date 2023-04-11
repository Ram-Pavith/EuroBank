using AutoMapper;
using Azure;
using EuroBankAPI.Data;
using Castle.Core.Resource;
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
        private readonly EuroBankContext _euroBankContext;
        public CustomerController(IUnitOfWork uw, ILogger<CustomerController> logger, IMapper mapper, IAuthService authService,EuroBankContext euroBankContext)
        {
            _uw = uw;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
            _euroBankContext = euroBankContext;
        }

        [HttpPost("CustomerAuthorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserAuthResponseDTO>> CustomerAuthorize(CustomerLoginDTO customerLogin)
        {
            _logger.LogInformation("CustomerAuthorization method is called");
            UserAuthResponseDTO response;
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(customerLogin);
                response = await _authService.AuthorizeEmployeeAndCustomer(request);
                if (response.Success)
                    return Ok(response);
                else
                    return BadRequest(response.Message);
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

        [HttpPost("CustomerLogin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDetailsDTO>> CustomerLogin(CustomerLoginDTO customerLogin)
        {
            _logger.LogInformation("CustomerLogin method is called");
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(customerLogin);
                var Customer = await _authService.CustomerLogin(request);
                if (Customer == null)
                {
                    return NotFound();
                }
                else
                {
                    var customerDetailsDTO = _mapper.Map<CustomerDetailsDTO>(Customer);
                    _logger.LogInformation("Customer logged in successfully");
                    return customerDetailsDTO;
                }
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

        [HttpGet("GetCustomerByCustomerId")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomerByCustomerId(string customerId)
        {
            var customer = await _uw.Employees.GetCustomerByCustomerId(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            List<CustomerDTO> customerDTO = _mapper.Map<List<CustomerDTO>>(customer);
            return customerDTO;
        }

        [HttpGet("GetCustomerById")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDetailsDTO>> GetCustomerById(string CustomerId)
        {
            _logger.LogInformation("Details of the customer of Id is "+CustomerId+"is called");
            try
            {
                var Customer = await _uw.Customers.GetAsync(x=>x.CustomerId == CustomerId);
                if (Customer == null)
                {
                    return NotFound();
                }
                else
                {
                    var customerDetailsDTO = _mapper.Map<CustomerDetailsDTO>(Customer);
                    _logger.LogInformation("Details of the customer of Id is " + CustomerId + "is got successfully");
                    return customerDetailsDTO;
                }
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



        [HttpGet("GetCustomerAccounts")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> getCustomerAccounts(string CustomerId)
        {
            _logger.LogInformation("Account details of the customer of Id is " + CustomerId + "is called");
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
                    _logger.LogInformation("Account details of the customer of Id is " + CustomerId + "is got successfully");
                    return AccountsDTO;
                }
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

        [HttpGet("GetAccount")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid AccountId)
        {
            _logger.LogInformation("Account details of AccountId " + AccountId + "is called");
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
                    _logger.LogInformation("Account details of AccountId " + AccountId + "is got successfully");
                    return targetAccountDTO;
                }
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

        [HttpGet("GetCustomerStatement")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StatementDTO>>> GetCustomerStatement(string CustomerId, DateTime? from_date, DateTime? to_date)
        {
            _logger.LogInformation("GetCustomerStatement method is called");
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
                _logger.LogInformation("CustomerStatement of "+CustomerId+" from "+from_date+ "to"+ to_date);
                return statementCustomerDTO;

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
        [HttpGet("ViewAllTransactions")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> ViewAllTransaction(string CustomerId)
        {
            _logger.LogInformation("View All transactions method is called");
            var targetAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
            if (targetAccounts == null)
            {
                _logger.LogError("Customer not found");
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
                _logger.LogInformation("ViewAllTransactions of the Customer of CustomerId: " + CustomerId + " is got successfully");
                return TransactionDTOs;
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


        
        [HttpDelete("RemoveCustomer")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> DeleteCustomer(string CustomerId)
        {
            _logger.LogInformation("DeleteEmployee method is called");

            var customerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (customerExists == null)
            {
                return BadRequest("Employee With the EmployeeId does not Exists");
            }
            await _uw.Customers.DeleteAsync(customerExists);
            _logger.LogInformation($"{CustomerId} deleted");
            return Ok(customerExists);
        }
    }
}
