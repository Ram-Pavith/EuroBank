using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefTransactionType
    {
        public RefTransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }
        
        [Key]
        public int TransactionTypeCode { get; set; } 
        public string TransactionTypeDescriptions { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }

    }
}
