using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class CounterPartyRepository :GenericRepository<CounterParty>,ICounterPartyRepository
    {
        private readonly EuroBankContext _db;

        public CounterPartyRepository(EuroBankContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CounterParty> UpdateAsync(CounterParty counterParty)
        {
            _db.CounterParties.Update(counterParty);
            await _db.SaveChangesAsync();
            return counterParty; 
        }
    }
}
