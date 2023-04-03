using EuroBankAPI.Data;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class ServiceRepository : GenericRepository<Models.Service>, IServiceRepository
    {
        private readonly EuroBankContext _db;

        public ServiceRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Models.Service> UpdateAsync(Models.Service service)
        {
            _db.Services.Update(service);
            await _db.SaveChangesAsync();
            return service;
        }
    
    }
}
