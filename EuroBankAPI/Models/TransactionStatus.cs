namespace EuroBankAPI.Models
{
    public class TransactionStatus
    {
        public int TransactionStatusId { get; set; }
        public string Message { get; set; } = string.Empty;
        public double SourceBalance { get; set; }
        public double DestinationBalance { get; set; }
        public virtual Account AccountId { get; set; }
    }
}
