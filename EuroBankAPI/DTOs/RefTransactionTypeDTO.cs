using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class RefTransactionTypeDTO
    {
        public RefTransactionTypeDTO()
        {
            Transactions = new HashSet<TransactionDTO>();
        }
        [Key]
        public int TransactionTypeCode { get; set; }
        public string TransactionTypeDescriptions { get; set; }
        public virtual ICollection<TransactionDTO>? Transactions { get; set; }
    }
}
