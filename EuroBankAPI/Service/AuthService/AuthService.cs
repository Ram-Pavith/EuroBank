using EuroBankAPI.Data;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EuroBankAPI.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly EuroBankContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(EuroBankContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserAuthResponseDTO> Login(UserAuthLoginDTO request)
        {
            var user = await _context.UsersAuth.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return new UserAuthResponseDTO { Message = "User not found." };
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new UserAuthResponseDTO { Message = "Wrong Password." };
            }
            if(request.Role != user.Role)
            {
                return new UserAuthResponseDTO { Message = "Authorization Error" };
            }

            string token = GenerateJWT(user);
            var refreshToken = CreateRefreshToken();
            SetRefreshToken(refreshToken, user);

            return new UserAuthResponseDTO
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token,
                TokenExpires = refreshToken.Expires
            };
        }

        public async Task<UserAuth> RegisterUser(UserAuthLoginDTO request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new UserAuth
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role,
            };

            _context.UsersAuth.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserAuthResponseDTO> RefreshToken()
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var user = await _context.UsersAuth.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                return new UserAuthResponseDTO { Message = "Invalid Refresh Token" };
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return new UserAuthResponseDTO { Message = "Token expired." };
            }

            string token = CreateToken(user);
            var newRefreshToken = CreateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            return new UserAuthResponseDTO
            {
                Success = true,
                Token = token,
                RefreshToken = newRefreshToken.Token,
                TokenExpires = newRefreshToken.Expires
            };
        }
        public string GenerateJWT(UserAuth userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Username ),
                new Claim(ClaimTypes.Role,userInfo.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(UserAuth user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken CreateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async void SetRefreshToken(RefreshToken refreshToken, UserAuth user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };
            _httpContextAccessor?.HttpContext?.Response
                .Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;

            await _context.SaveChangesAsync();
        }
    }
}

