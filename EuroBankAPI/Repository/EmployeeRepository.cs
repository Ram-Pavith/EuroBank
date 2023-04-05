using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly EuroBankContext _db;

        public EmployeeRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
            return employee;
        }
    }
}
