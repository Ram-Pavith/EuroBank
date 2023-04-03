using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerDTO
    {
        public string CustomerId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string PanNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public virtual CustomerCreationStatus CustomerCreationStatus { get; set; }
    }
}
