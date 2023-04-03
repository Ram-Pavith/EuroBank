using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class RefPaymentMethodRepository:GenericRepository<RefPaymentMethod>,IRefPaymentMethodRepository
    {
        private readonly EuroBankContext _db;

        public RefPaymentMethodRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<RefPaymentMethod> UpdateAsync(RefPaymentMethod RefPaymentMethod)
        {
            _db.RefPaymentMethods.Update(RefPaymentMethod);
            await _db.SaveChangesAsync();
            return RefPaymentMethod;
        }
    }
}
