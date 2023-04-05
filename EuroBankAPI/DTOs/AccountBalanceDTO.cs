namespace EuroBankAPI.DTOs
{
    public class AccountBalanceDTO
    {
        public Guid AccountId { get; set; } = Guid.NewGuid();
        public double Balance { get; set; }
    }
}
