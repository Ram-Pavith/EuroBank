using EuroBankAPI.DTOs;
using EuroBankAPI.Service.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EuroBankAPI.Controllers
{
    public class BusinessRulesAndServiceController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<BusinessRulesAndServiceController> _logger;
        private readonly IConfiguration _config;
        public BusinessRulesAndServiceController(IEmailService emailService, ILogger logger, IConfiguration config) {
            _emailService = emailService;
            logger = _logger;
            _config = config;
        }

        [HttpPost("SendEmail")]
        [Authorize(Roles = "Customer,Employee")]
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
    }
}
