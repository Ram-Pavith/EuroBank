using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.DTOs
{
    public class StatementDTO
    {
        public int StatementId { get; set; }
        [ForeignKey("AccountDTO")]
        public Guid AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; } = string.Empty;
        public string RefNo { get; set; } = string.Empty;

        public DateTime ValueDate { get; set; }
        public double Withdrawal { get; set; }
        public double Deposit { get; set; }
        public double ClosingBalance { get; set; }
        public virtual AccountDTO Account { get; set; } = null!;
    }
}
