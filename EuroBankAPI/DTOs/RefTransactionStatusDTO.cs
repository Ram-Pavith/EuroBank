using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class RefTransactionStatusDTO
    {
        public string TransactionStatusCode { get; set; }
        public string TransactionStatusDescriptions { get; set; }

        public virtual TransactionDTO Transaction { get; set; }
    }
}
