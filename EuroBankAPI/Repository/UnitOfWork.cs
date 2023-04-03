using EuroBankAPI.Data;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly EuroBankContext _db;
        public UnitOfWork(EuroBankContext db)
        {
            _db = db;
            Employees = new EmployeeRepository(db);

        }
        public IEmployeeRepository Employees { get; }

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
