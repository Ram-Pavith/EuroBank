using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using System.Transactions;

namespace EuroBankAPI.Repository
{
    public class TransactionStatusRepository:GenericRepository<Models.TransactionStatus>,ITransactionStatusRepository
    {
        private readonly EuroBankContext _db;
        public TransactionStatusRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Models.TransactionStatus> UpdateAsync(Models.TransactionStatus transactionStatus)
        {
            _db.TransactionStatuses.Update(transactionStatus);
            await _db.SaveChangesAsync();
            return transactionStatus;
        }

    }
}
