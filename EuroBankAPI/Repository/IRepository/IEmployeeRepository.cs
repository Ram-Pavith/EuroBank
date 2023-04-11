using System.Threading.Tasks;
using EuroBankAPI.Models;


namespace EuroBankAPI.Repository.IRepository
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        Task<Employee> UpdateAsync(Employee employee);
        Task<IQueryable<Customer>> GetCustomerByCustomerId(string customerId);

    }
}