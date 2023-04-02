using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }
        public string EmailId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PanNumber { get; set; }
        public DateTime DOB { get; set; }
    }
}
