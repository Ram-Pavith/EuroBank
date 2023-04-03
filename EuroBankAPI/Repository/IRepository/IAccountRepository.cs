using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IAccountRepository:IGenericRepository<Account>
    {
        Task<Account> UpdateAsync(Account account);

    }
}
