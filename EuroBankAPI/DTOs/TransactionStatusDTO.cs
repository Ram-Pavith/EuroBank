using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.DTOs
{
    public class TransactionStatusDTO
    {
        public int TransactionStatusId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }
        public string Message { get; set; } = string.Empty;
        public double SourceBalance { get; set; }
        public virtual AccountDTO Account { get; set; } = null!;
    }
}
