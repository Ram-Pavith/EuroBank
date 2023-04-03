using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [StringLength(10,MinimumLength =10)]
        [Display(Name ="Phone Number")]
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PanNumber { get; set; }
        public DateTime DOB { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
