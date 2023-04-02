namespace EuroBankAPI.Models
{
    public class AccountCreationStatus
    {
        public int AccountCreationStatusId { get; set; }
        
        public string Message { get; set; } = string.Empty;

        public Account Account { get; set; }
    }
}
