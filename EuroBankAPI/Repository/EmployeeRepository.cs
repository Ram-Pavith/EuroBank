using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IQueryable<Customer>> GetCustomerByCustomerId(string customerId)
        {
            var res = _db.Customers.FromSqlRaw($"exec GetCustomerByCustomerId @customerId='{customerId}'");
            return res;
        }

        public async Task<IQueryable<Customer>> GetCustomerByCustomerId(string customerId)
        {
            var res = _db.Customers.FromSqlRaw($"exec GetCustomerByCustomerId @customerId='{customerId}'");
            return res;
        }

    }
}
