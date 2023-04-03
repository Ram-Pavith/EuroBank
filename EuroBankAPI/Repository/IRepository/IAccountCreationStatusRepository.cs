using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IAccountCreationStatusRepository:IGenericRepository<AccountCreationStatus>
    {
        Task<AccountCreationStatus> UpdateAsync(AccountCreationStatus accountCreationStatus);
    }
}
