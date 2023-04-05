using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class TransactionRepository:GenericRepository<Transaction> ,ITransactionRepository
    {
        private readonly EuroBankContext _db;

        public TransactionRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
            return transaction;
        }
    }
}
