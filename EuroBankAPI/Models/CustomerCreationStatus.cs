using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerCreationStatus
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Message { get; set; }
        public virtual Customer Customer { get; set; }


    }
}
