using EuroBankAPI.Data;
using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.EntitiesRepo
{
    public class AccountRepo : GenericRepository<Account>, IAccountRepo
    {
        private readonly EuroBankContext _db;

        public AccountRepo(EuroBankContext db) : base(db) 
        { 
            _db = db;
        }


    }
}
