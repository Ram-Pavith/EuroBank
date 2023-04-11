using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Authorization;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using EuroBankAPI.Helpers;
using AutoMapper.Execution;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _uw;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public EmployeeController(IUnitOfWork uw,ILogger<EmployeeController> logger ,IMapper mapper,IAuthService authService) { 
            _uw= uw;
            _logger= logger;
            _mapper= mapper;
            _authService= authService;
        }

        [HttpPost("EmployeeRegister")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDTO>> EmployeeRegister(EmployeeRegisterDTO employeeRegisterDTO)
        {
            _logger.LogInformation("EmployeeRegisteration method is called ");
            try
            {
                var employeeDTO = _mapper.Map<EmployeeDTO>(employeeRegisterDTO);
                Employee employee = _mapper.Map<Employee>(employeeDTO);
                var employeeExists = await _uw.Employees.GetAsync(x => x.EmailId == employeeRegisterDTO.EmailId);
                if(employeeExists != null)
                {
                    _logger.LogError("Employee already exists with the same email id, try with another email id");
                    return BadRequest("Employee already exists with the same email id, try with another email id");
                }
                _authService.CreatePasswordHash(employeeRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
                employee.PasswordHash = passwordHash;
                employee.PasswordSalt = passwordSalt;
                
                await _uw.Employees.CreateAsync(employee);
                return employeeDTO;

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

        [HttpPost("CreateCustomer")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerCreationStatusDTO>> CreateCustomer(CustomerRegisterDTO customerRegisterDTO)
        {
            _logger.LogInformation("CreateCustomer method is called");
            var customerDTO = _mapper.Map<CustomerDTO>(customerRegisterDTO);
            _authService.CreatePasswordHash(customerRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            customerDTO.PasswordHash = passwordHash;
            customerDTO.PasswordSalt = passwordSalt;
            try
            {
                Customer customer = _mapper.Map<Customer>(customerDTO);
                var customerExists = await _uw.Customers.GetAsync(x => x.CustomerId== customerRegisterDTO.CustomerId || x.EmailId  == customerRegisterDTO.EmailId);
                if (customerExists != null) {
                    _logger.LogError("Customer Exists with the same CustomerId, please try to register with a different customer id");
                    return BadRequest("Customer Exists with the same CustomerId, please try to register with a different customer id");
                }
                CustomerCreationStatus customerCreationStatus;
                try
                {
                    Account account = new Account()
                    {
                        DateCreated = DateTime.Now,
                        Balance = 10000
                    };
                    customerCreationStatus = new CustomerCreationStatus()
                    {
                        Message = "Success"
                    };
                    try
                    {

                        account.AccountCreationStatusId = 1;
                        account.AccountTypeId = 1;
                        account.CustomerId = customer.CustomerId;
                        customer.CustomerCreationStatusId = 1;
                        customer.Accounts.Add(account);
                        await _uw.Customers.CreateAsync(customer);
                        await _uw.Accounts.CreateAsync(account);

                    }
                    catch (Exception ex)
                    {
                        AccountCreationStatus accountCreationStatus = new AccountCreationStatus()
                        {
                            Message = "Failure"
                        };
                    }
                }
                catch (Exception ex)
                {
                    customerCreationStatus = new CustomerCreationStatus()
                    {
                        Message = "Failure"
                    };
                }
                CustomerCreationStatusDTO customerCreationStatusDTO = _mapper.Map<CustomerCreationStatusDTO>(customerCreationStatus);
                _logger.LogInformation("New customer created successfully");
                return customerCreationStatusDTO;

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

        [HttpPost("EmployeeAuthorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserAuthResponseDTO>> EmployeeAuthorize(EmployeeLoginDTO employeeLogin)
        {
            _logger.LogInformation("EmployeeAutherization is done");
            UserAuthResponseDTO response;
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(employeeLogin);
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

        [HttpPost("EmployeeLogin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDetailsDTO>> EmployeeLogin(EmployeeLoginDTO employeeLogin)
        {
            _logger.LogInformation("EmployeeLogin method is called");
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(employeeLogin);
                var Employee = await _authService.EmployeeLogin(request);
                if (Employee == null)
                {
                    return NotFound();
                }
                else
                {
                    var employeeDetailsDTO = _mapper.Map<EmployeeDetailsDTO>(Employee);
                    _logger.LogInformation("Employee logged in successfully");
                    return employeeDetailsDTO;
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

        [HttpGet("ViewAllTransactions")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> ViewAllTransactions(int PageSize = 0, int PageNumber = 1)
        {
            _logger.LogInformation("ViewAllTransactions method is called ");
            IEnumerable<Transaction> Transactions;
            try
            {
                if (PageSize <= 0)
                {
                    _logger.LogError("page size is less than 0");
                    Transactions = await _uw.Transactions.GetAllAsync();
                }
                else
                {
                    _logger.LogInformation("calling view all transactions");
                    Transactions = await _uw.Transactions.GetAllAsync(pageSize: PageSize, pageNumber: PageNumber);
                }
                List<TransactionDTO> TransactionDTOs = _mapper.Map<List<TransactionDTO>>(Transactions);
                _logger.LogInformation("ViewAllTransactions method is successfully executed ");
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

        [HttpGet("ViewAllBankAccounts")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> ViewAllBankAccounts(int PageSize = 0, int PageNumber = 1)
        {
            _logger.LogInformation("ViewAllBankAccounts method is called ");
            IEnumerable<Account> BankAccounts;
            try
            {
                if (PageSize <= 0)
                {
                    BankAccounts = await _uw.Accounts.GetAllAsync();
                }
                else
                {
                    BankAccounts = await _uw.Accounts.GetAllAsync(pageSize: PageSize, pageNumber: PageNumber);
                }
                List<AccountDTO> AccountsDTOs = _mapper.Map<List<AccountDTO>>(BankAccounts);
                _logger.LogInformation("ViewAllBankAccounts method is successfully executed ");
                return AccountsDTOs;
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

        [HttpGet("GetAllCustomers")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerDetailsDTO>>> GetAllCustomers(int PageSize = 0, int PageNumber = 1)
        {
            _logger.LogInformation("GetAllCustomers method is called ");
            IEnumerable<Customer> Customers;
            try
            {
                if (PageSize <= 0)
                {
                    Customers = await _uw.Customers.GetAllAsync();
                }
                else
                {
                    Customers = await _uw.Customers.GetAllAsync(pageSize: PageSize, pageNumber: PageNumber);
                }
                List<CustomerDTO> CustomersDTOs = _mapper.Map<List<CustomerDTO>>(Customers);
                _logger.LogInformation("GetAllCustomers method is successfully executed ");
                return CustomersDTOs;
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

        [HttpGet("AllAccountPages")]
        //[Authorize(Roles = "Employee,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AllAccountPages(int pageSize)
        {
            decimal count = await _uw.Accounts.CountAsync();
            return Convert.ToInt32(Math.Ceiling((decimal)(count / pageSize)));
        }

        [HttpGet("AllTransactionPages")]
        //[Authorize(Roles = "Employee,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AllTransactionPages(int pageSize)
        {
            decimal count = await _uw.Transactions.CountAsync();
            return Convert.ToInt32(Math.Ceiling((decimal)(count / pageSize)));
        }

        [HttpDelete("RemoveEmployee")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> DeleteEmployee(Guid EmployeeId)
        {
            _logger.LogInformation("DeleteEmployee method is called");
            var employeeExists = await _uw.Employees.GetAsync(x => x.EmployeeId == EmployeeId);
            if(employeeExists == null)
            {

                return BadRequest("Employee With the EmployeeId does not Exists");
            }
            await _uw.Employees.DeleteAsync(employeeExists);
            _logger.LogInformation($"{EmployeeId} deleted");
            return Ok(employeeExists);
        }
    }
}
