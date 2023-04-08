using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly EuroBankContext _db;

        public CustomerRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
            return customer;
        }
    }
}
