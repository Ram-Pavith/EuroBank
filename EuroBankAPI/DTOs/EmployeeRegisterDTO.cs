namespace EuroBankAPI.DTOs
{
    public class EmployeeRegisterDTO
    {
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        public string EmailId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
    }
}
