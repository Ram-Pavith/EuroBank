using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class RefTransactionStatusDTO
    {
        public RefTransactionStatusDTO() { 
            Transactions = new HashSet<TransactionDTO>();
        }
        public int TransactionStatusCode { get; set; }
        public string TransactionStatusDescriptions { get; set; }

        public virtual ICollection<TransactionDTO>? Transactions { get; set; }
    }
}
