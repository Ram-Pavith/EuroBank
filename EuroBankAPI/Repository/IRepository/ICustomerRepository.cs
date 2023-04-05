using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ICustomerRepository: IGenericRepository<Customer>
    {
        Task<Customer> UpdateAsync(Customer customer);
    }
}
