using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Repository
{
    public class AccountCreationStatusRepository : GenericRepository<AccountCreationStatus>, IAccountCreationStatusRepository
    {
        private readonly EuroBankContext _db;
        public AccountCreationStatusRepository(EuroBankContext db):base(db)
        {
            _db = db;
        }

        public async Task<AccountCreationStatus> UpdateAsync(AccountCreationStatus accountCreationStatus)
        {
            _db.AccountCreationStatuses.Update(accountCreationStatus);
            await _db.SaveChangesAsync();
            return accountCreationStatus;
        }
    }
}
