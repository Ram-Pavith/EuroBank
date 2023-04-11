using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using EuroBankAPI.Service.BusinessService;
using EuroBankAPI.Service.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Controllers 
{
    public class BusinessRulesAndServiceController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<BusinessRulesAndServiceController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _uw;
        private readonly IBusinessService _businessService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public BusinessRulesAndServiceController(IEmailService emailService, ILogger<BusinessRulesAndServiceController> logger, IConfiguration config, IUnitOfWork uw,IBusinessService businessService,IMapper mapper,IAuthService authService) {
            _emailService = emailService;
            _logger = logger;
            _config = config;
            _uw = uw;
            _businessService = businessService;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("SendEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SendEmail(EmailDTO Request, string Role)
        {
            _logger.LogInformation("Send Email method is called");
            try
            {
                if (Role == "Employee")
                {
                    var employeeExists = await _uw.Employees.GetAsync(e => e.EmailId == Request.To);
                    if (employeeExists != null)
                    {
                        _emailService.SendEmail(Request);
                    }
                    else
                    {
                        return BadRequest("Employee not Found");
                    }
                }
                if (Role == "Customer")
                {
                    var customerExists = await _uw.Customers.GetAsync(c => c.EmailId == Request.To);
                    if (customerExists != null)
                    {
                        _emailService.SendEmail(Request);
                    }
                    else
                    {
                        return BadRequest("Customer not Found");
                    }
                }
                return Ok("{\"success\": true}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ResetPassword(string Email, string Password, string Role)
        {
            try
            {
                if(Role == "Customer")
                {
                    Customer customer = await _uw.Customers.GetAsync(x => x.EmailId == Email);
                    _authService.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
                    customer.PasswordHash = passwordHash;
                    customer.PasswordSalt = passwordSalt;
                    await _uw.Customers.UpdateAsync(customer);
                    _uw.Save();
                    return Ok("{\"success\": true}");
                }
                if(Role == "Employee")
                {
                    Employee employee = await _uw.Employees.GetAsync(x => x.EmailId == Email);
                    _authService.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
                    employee.PasswordHash = passwordHash;
                    employee.PasswordSalt = passwordSalt;
                    await _uw.Employees.UpdateAsync(employee);
                    _uw.Save();
                    return Ok("{\"success\": true}");
                }
                return BadRequest("Role not Recognised");
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

        [HttpPost("EvaluateMinBalance")]
        //[Authorize(Roles ="Customer,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RuleStatus>> EvaluateMinBalance(Guid AccountId, double Amount)
        {
            try
            {
                _logger.LogInformation("Evaluate Mininmum balance method is called");
                var accountExists = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
                var ruleStatus = new RuleStatus();
                if (accountExists == null)
                {
                    ruleStatus = new RuleStatus() { RuleStatusId = 3, Status = "Account does not exist, enter correct account number" };
                    return ruleStatus;
                }
                var remainingBalance = accountExists.Balance - Amount;
                double minBalance = _businessService.EvaluateMinBalance(accountExists.AccountTypeId);
                if (accountExists.Balance > minBalance && remainingBalance > minBalance)
                {
                    ruleStatus = new RuleStatus() { RuleStatusId = 1, Status = "Minimum Balance Sufficient" };
                    return ruleStatus;
                }
                if (accountExists.Balance > Amount && remainingBalance >= 0 && remainingBalance < minBalance)
                {
                    ruleStatus = new RuleStatus() { RuleStatusId = 2, Status = "Balance sufficient, but closing balance is less than minimum balance" };
                    return ruleStatus;
                }
                return new RuleStatus() { RuleStatusId = 4, Status = "Balance insufficient to proceed with the transaction" };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EvaluateServiceCharges")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> EvaluateServiceCharges()
        {
            try
            {
                            _logger.LogInformation("Evaluate service Charges method is called");
                var count = 0;
                var accounts = await _uw.Accounts.GetAllAsync();
                foreach (var acc in accounts)
                {
                    if (_businessService.EvaluateMinBalance(acc.AccountTypeId) > acc.Balance)
                    {
                        acc.Balance -= _businessService.ServiceCharges(acc.AccountTypeId);
                        await _uw.Accounts.UpdateAsync(acc);
                        count++;
                    }
                }
                return count;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
