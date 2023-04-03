using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ICounterPartyRepository:IGenericRepository<CounterPartyRepository>
    {
        Task<CounterParty> UpdateAsync(CounterParty counterParty);
    }
}
