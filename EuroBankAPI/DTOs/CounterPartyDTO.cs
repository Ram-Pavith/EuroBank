using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class CounterPartyDTO
    {
        public CounterPartyDTO()
        {
            Transactions = new HashSet<TransactionDTO>();
        }
        public string CounterPartyId { get; set; }
        public string CounterPartyName { get; set; }
        public virtual ICollection<TransactionDTO> Transactions { get; set; }
    }
}
