using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IRefTransactionTypeRepository:IGenericRepository<RefTransactionType>
    {
        Task<RefTransactionType> UpdateAsync(RefTransactionType RefTransactionType);
    }
}
