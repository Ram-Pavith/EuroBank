using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerCreationStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string Message { get; set; }
        public virtual Customer Customer { get; set; }


    }
}
