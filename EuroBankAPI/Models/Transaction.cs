using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        
        public string CounterPartyId { get; set; }
        public int ServiceId { get; set; }
        public  string TransactionStatusCode { get; set; }
        public int TransactionTypeCode { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public double AmountOfTransaction { get; set; }
        public virtual CounterParties CounterParties { get; set; }
        public virtual Services Services { get; set; }
        public virtual RefTransactionStatus RefTransactionStatus { get; set; }
        public virtual RefTransactionTypes RefTransactionTypes { get; set; }


    }
}
