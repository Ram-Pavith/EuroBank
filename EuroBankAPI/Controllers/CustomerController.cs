using AutoMapper;
using Azure;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public CustomerController(IUnitOfWork context, ILogger<CustomerController> logger, IMapper mapper,IAuthService authService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<UserAuthResponseDTO>> CustomerLogin(CustomerLoginDTO customerLogin)
        {
            UserAuthResponseDTO response;
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(customerLogin);
                response = await _authService.Login(request);
                if (response.Success)
                    return Ok(response);
                else 
                    return BadRequest(response.Message);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
