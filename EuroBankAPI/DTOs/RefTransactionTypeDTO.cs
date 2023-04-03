using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class RefTransactionTypeDTO
    {
        [Key]
        public int TransactionTypeCode { get; set; }
        public string TransactionTypeDescriptions { get; set; }
        public virtual TransactionDTO Transaction { get; set; }
    }
}
