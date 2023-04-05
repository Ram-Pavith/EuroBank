using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class AccountRepository:GenericRepository<Account>,IAccountRepository
    {
        private EuroBankContext _db;
        public AccountRepository(EuroBankContext db):base(db) {
            _db = db;
        }

        public async Task<Account> UpdateAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
            return account;
        }
    }
}
