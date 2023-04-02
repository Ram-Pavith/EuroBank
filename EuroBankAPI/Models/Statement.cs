namespace EuroBankAPI.Models
{
    public class Statement
    {
        public int StatementId { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; } = string.Empty;
        public string RefNo { get; set; } = string.Empty;

        public DateTime ValueDate { get; set; }
        public double Withdrawal { get; set; }
        public double Deposit { get; set; }
        public double ClosingBalance { get; set; }
        public virtual Account AccountId { get; set; }
    }
}
