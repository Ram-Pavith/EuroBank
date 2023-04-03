using EuroBankAPI.DTOs;
using EuroBankAPI.Models;

namespace EuroBankAPI.Service.AuthService
{
    public interface IAuthService
    {
        Task<UserAuth> RegisterUser(UserAuthLoginDTO request);
        Task<UserAuthResponseDTO> Login(UserAuthLoginDTO request);
        Task<UserAuthResponseDTO> RefreshToken();
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

    }
}
