using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class TransactionStatus
    {
        public int TransactionStatusId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }
        public string Message { get; set; } = string.Empty;
        public double SourceBalance { get; set; }
        public virtual Account? Account { get; set; } = null!;
    }
}
