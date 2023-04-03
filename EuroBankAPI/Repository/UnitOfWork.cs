using EuroBankAPI.Data;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Repository
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly EuroBankContext _db;
        public UnitOfWork(EuroBankContext db)
        {
            _db = db;
            Employees = new EmployeeRepository(db);
            UserAuths = new UserAuthRepository(db);

        }
        public IEmployeeRepository Employees { get; }
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
