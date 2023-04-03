using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class AccountTypeRepository : GenericRepository<AccountType>, IAccountTypeRepository
    {
        private readonly EuroBankContext _db;
        public AccountTypeRepository(EuroBankContext db):base(db) 
        {
            _db = db;
        }

        public async Task<AccountType> UpdateAsync(AccountType accountType)
        {
            _db.AccountTypes.Update(accountType);
            await _db.SaveChangesAsync();
            return accountType;
        }
    }
}
