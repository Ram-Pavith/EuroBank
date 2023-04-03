namespace EuroBankAPI.Models
{
    public class CounterParty
    {
        public CounterParty()
        {

        }
        public string CounterPartyId { get; set; }
        public string CounterPartyName { get; set;}
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
