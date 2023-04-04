using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface ICounterPartyRepository:IGenericRepository<CounterParty>
    {
        Task<CounterParty> UpdateAsync(CounterParty counterParty);
    }
}
