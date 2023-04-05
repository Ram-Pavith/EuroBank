using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefTransactionStatus
    {
        public RefTransactionStatus() {
            Transactions = new HashSet<Transaction>();
        }
        public int TransactionStatusCode { get; set; } 
        public string TransactionStatusDescriptions { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
