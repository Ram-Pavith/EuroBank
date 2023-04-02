using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class UserAuthRepository : GenericRepository<UserAuth>, IUserAuthRepository
    {
        private readonly EuroBankContext _db;

        public UserAuthRepository(EuroBankContext db): base(db)
        {
            _db = db;
        }

        public async Task<UserAuth> UpdateAsync(UserAuth userAuth)
        {
            _db.UserAuths.Update(userAuth);
            await _db.SaveChangesAsync();
            return userAuth;
        }
    }
}
