using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerCreationStatus
    {
        [Key]
        public int CustomerCreationId { get; set; }
        [Required]
        public string Message { get; set; }
        //public virtual Customer Customer { get; set; }
    }
}
