using EuroBankAPI.DTOs;
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
        private readonly IAuthService _authService;

        public AuthorizeController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<ActionResult<UserAuthDTO>> RegisterUser(UserAuthLoginDTO request)
        {
            var response = await _authService.RegisterUser(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserAuthDTO>> Login(UserAuthLoginDTO request)
        {
            var response = await _authService.Login(request);
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var response = await _authService.RefreshToken();
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        public ActionResult<string> Aloha()
        {
            return Ok("Aloha! You're authorized!");
        }
    }
}
