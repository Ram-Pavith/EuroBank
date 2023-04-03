using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class RefTransactionTypeRepository:GenericRepository<RefTransactionType>,IRefTransactionTypeRepository
    {
        private readonly EuroBankContext _db;

        public RefTransactionTypeRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<RefTransactionType> UpdateAsync(RefTransactionType RefTransactionType)
        {
            _db.RefTransactionType.Update(RefTransactionType);
            await _db.SaveChangesAsync();
            return RefTransactionType;
        }
    }
}
