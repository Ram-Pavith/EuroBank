using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class CustomerDetailsDTO
    {
        [Key]
        public string CustomerId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailId { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string? PanNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }
}
