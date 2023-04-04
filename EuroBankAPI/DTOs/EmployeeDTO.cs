using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class EmployeeDTO
    {
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        public string EmailId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

