namespace EuroBankAPI.Models
{
    public class AccountCreationStatus
    {
        public int AccountCreationStatusId { get; set; }
        
        public Guid AccountId { get; set; }
        public string Message { get; set; } = string.Empty;

        public virtual Account Account { get; set; } = null!;
    }
}
