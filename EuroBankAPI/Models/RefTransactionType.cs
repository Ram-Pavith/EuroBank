using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefTransactionType
    {
        
        [Key]
        public int TransactionTypeCode { get; set; } 
        public string TransactionTypeDescriptions { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}
