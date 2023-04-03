using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IRefPaymentMethodRepository:IGenericRepository<RefPaymentMethod>
    {
        Task<RefPaymentMethod> UpdateAsync(RefPaymentMethod RefPaymentMethod);
    }
}
