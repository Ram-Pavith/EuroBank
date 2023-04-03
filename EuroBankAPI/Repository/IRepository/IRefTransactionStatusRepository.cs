using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IRefTransactionStatusRepository:IGenericRepository<RefTransactionStatus>
    {
        Task <RefTransactionStatus> UpdateAsync(RefTransactionStatus RefTransactionStatus);
    }
}
