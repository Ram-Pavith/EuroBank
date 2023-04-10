using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.BusinessService;
using EuroBankAPI.Service.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
        public BusinessRulesAndServiceController(IEmailService emailService, ILogger<BusinessRulesAndServiceController> logger, IConfiguration config, IUnitOfWork uw,IBusinessService businessService,IMapper mapper) {
            _emailService = emailService;
            logger = _logger;
            _config = config;
            _uw = uw;
            _businessService = businessService;
            _mapper = mapper;
        }

        [HttpPost("SendEmail")]
        //[Authorize(Roles = "Customer,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SendEmail(EmailDTO Request)
        {
            _logger.LogInformation("Send Email method is called");
            try
            {
                _emailService.SendEmail(Request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
            if(accountExists.Balance > minBalance && remainingBalance>minBalance)
            {
                ruleStatus = new RuleStatus() { RuleStatusId = 1,Status = "Minimum Balance Sufficient"};

                return ruleStatus;
            }
            if(accountExists.Balance>Amount && remainingBalance >= 0 && remainingBalance < minBalance) 
            {
                ruleStatus = new RuleStatus() { RuleStatusId = 2, Status = "Balance sufficient, but closing balance is less than minimum balance" };
                return ruleStatus;
            }
            return new RuleStatus() { RuleStatusId = 4, Status= "Balance insufficient to proceed with the transaction" };
        }

        [HttpPost("EvaluateServiceCharges")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EvaluateServiceCharges()
        {
            _logger.LogInformation("Evaluate service Charges method is called");
            var accounts = await _uw.Accounts.GetAllAsync();
            foreach(var acc in accounts)
            {
                if (_businessService.EvaluateMinBalance(acc.AccountTypeId)>acc.Balance)
                {
                    acc.Balance -= _businessService.ServiceCharges(acc.AccountTypeId);
                    await _uw.Accounts.UpdateAsync(acc);
                }
            }
            return Ok();
        }
    }
}
