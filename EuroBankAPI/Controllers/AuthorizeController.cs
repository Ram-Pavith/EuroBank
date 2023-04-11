using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthorizeController(IAuthService authService,IMapper mapper)
        {
            _mapper = mapper;
            _authService = authService;

        }
        [HttpPost("RegisterAuth")]
        public async Task<ActionResult<UserAuth>> RegisterUser(UserAuthLoginDTO request)
        {
            _logger.LogInformation("Register User in the Autherization controller is called");
            var response = await _authService.RegisterUser(request);
            return Ok(response);
        }

        [HttpPost("Authorize")]
        public async Task<ActionResult<UserAuth>> Authorize(UserAuthLoginDTO request)
        {
            //var request = _mapper.Map<UserAuthLoginDTO>(emplogin);
            var response = await _authService.Login(request);
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var response = await _authService.RefreshToken();
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet, Authorize(Roles = "Customer,Employee")]
        public ActionResult<string> Aloha()
        {
            return Ok("Aloha! You're authorized!");
        }
    }
}
