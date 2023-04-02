using EuroBankAPI.DTOs;
using EuroBankAPI.Models;

namespace EuroBankAPI.Service.AuthService
{
    public interface IAuthService
    {
        Task<UserAuth> RegisterUser(UserAuthLoginDTO request);
        Task<UserAuthResponseDTO> Login(UserAuthLoginDTO request);
        Task<UserAuthResponseDTO> RefreshToken();
    }
}
