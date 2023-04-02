using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefTransactionStatus
    {
        
        public string TransactionStatusCode { get; set; } 
        public string TransactionStatusDescriptions { get; set; }
    }
}
