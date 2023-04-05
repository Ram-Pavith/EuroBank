using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class CustomerCreationStatusRepository:GenericRepository<CustomerCreationStatus>,ICustomerCreationStatusRepository
    {
        private readonly EuroBankContext _db;

        public CustomerCreationStatusRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CustomerCreationStatus> UpdateAsync(CustomerCreationStatus customerCreationStatus)
        {
            _db.Update(customerCreationStatus);
            await _db.SaveChangesAsync();
            return customerCreationStatus;
        }
    }
}
