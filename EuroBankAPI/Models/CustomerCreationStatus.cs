using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerCreationStatus
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Customer.CustomerId))]
        public string CustomerId { get; set; }
        public string Message { get; set; }

    }
}
