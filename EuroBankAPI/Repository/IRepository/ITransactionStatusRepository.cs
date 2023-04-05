using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ITransactionStatusRepository:IGenericRepository<TransactionStatus>
    {
        Task<Models.TransactionStatus> UpdateAsync(Models.TransactionStatus transactionStatus);
    }
}
