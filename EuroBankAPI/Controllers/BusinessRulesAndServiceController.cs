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
        public BusinessRulesAndServiceController(IEmailService emailService, ILogger<BusinessRulesAndServiceController> logger, IConfiguration config, IUnitOfWork uw,IBusinessService businessService) {
            _emailService = emailService;
            logger = _logger;
            _config = config;
            _uw = uw;
            _businessService = businessService;
        }

        [HttpPost("SendEmail")]
        //[Authorize(Roles = "Customer,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SendEmail(EmailDTO Request)
        {
            try
            {
                _emailService.SendEmail(Request);
                return Ok();
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
            var accountExists = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            var ruleStatus = new RuleStatus();
            if (accountExists == null)
            {
                ruleStatus = new RuleStatus() { RuleStatusId = 3, Status = "Account does not exist, enter correct account number" };
                return ruleStatus;
            }
            var remainingBalance = accountExists.Balance - Amount;
            double minBalance = _businessService.EvaluateMinBalance(accountExists.AccountTypeId, accountExists.Balance);
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
    }
}
