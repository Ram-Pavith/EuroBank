using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly EuroBankContext _db;
        public UnitOfWork(EuroBankContext db)
        {
            _db = db;
            UserAuths = new UserAuthRepository(db);
        }
        public IUserAuthRepository UserAuths { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
