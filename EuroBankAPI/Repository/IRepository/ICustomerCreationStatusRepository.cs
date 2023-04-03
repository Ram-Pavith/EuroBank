using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ICustomerCreationStatusRepository : IGenericRepository<CustomerCreationStatus>
    {
        Task<CustomerCreationStatus> UpdateAsync(CustomerCreationStatus customerCreationStatus);
    }
}
