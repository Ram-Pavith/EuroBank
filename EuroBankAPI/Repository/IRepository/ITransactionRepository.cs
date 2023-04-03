using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ITransactionRepository:IGenericRepository<Transaction>
    {
        Task<Transaction> UpdateAsync(Transaction transaction);
    }
}
