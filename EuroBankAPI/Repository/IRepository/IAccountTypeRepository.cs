using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IAccountTypeRepository:IGenericRepository<AccountType>
    {
        Task<AccountType> UpdateAsync(AccountType accountType);
    }
}
