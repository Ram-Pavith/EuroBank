using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class CustomerRegisterDTO
    {
        [Key]
        public string CustomerId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string PanNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public virtual CustomerCreationStatusDTO CustomerCreationStatus { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
