using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class CounterPartyDTO
    {
        public CounterPartyDTO()
        {
            Transactions = new HashSet<Transaction>();
        }
        public string CounterPartyId { get; set; }
        public string CounterPartyName { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
