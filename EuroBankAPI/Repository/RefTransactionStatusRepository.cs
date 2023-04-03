using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class RefTransactionStatusRepository:GenericRepository<RefTransactionStatus>, IRefTransactionStatusRepository
    {
        private readonly EuroBankContext _db;

        public RefTransactionStatusRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<RefTransactionStatus> UpdateAsync(RefTransactionStatus RefTransactionStatus)
        {
            _db.RefTransactionStatuses.Update(RefTransactionStatus);
            await _db.SaveChangesAsync();
            return RefTransactionStatus;
        }
    }
}
