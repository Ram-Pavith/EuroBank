using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IUserAuthRepository: IGenericRepository<UserAuth>
    {
        Task<UserAuth> UpdateAsync(UserAuth userAuth);
    }
}
