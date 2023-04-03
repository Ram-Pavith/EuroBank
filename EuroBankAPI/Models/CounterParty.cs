namespace EuroBankAPI.Models
{
    public class CounterParty
    {
        public CounterParty()
        {
            Transactions=new HashSet<Transaction>();
        }
        public string CounterPartyId { get; set; }
        public string CounterPartyName { get; set;}
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
