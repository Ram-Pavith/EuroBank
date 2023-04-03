using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerCreationStatusDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string Message { get; set; }
        public virtual CustomerDTO Customer { get; set; }

    }
}
