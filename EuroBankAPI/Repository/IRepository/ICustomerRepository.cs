using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ICustomerRepository: IGenericRepository<Customer>
    {
        //void GetAsync(Func<Customer, bool> value, string v);
        Task<Customer> UpdateAsync(Customer customer);

    }
}
