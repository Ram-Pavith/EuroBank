using EuroBankAPI.DTOs;

namespace EuroBankAPI.Service.AuthService
{
    public interface IAuthService
    {
        Task<UserAuthDTO> RegisterUser(UserAuthLoginDTO request);
        Task<UserAuthResponseDTO> Login(UserAuthLoginDTO request);
        Task<UserAuthResponseDTO> RefreshToken();
    }
}
