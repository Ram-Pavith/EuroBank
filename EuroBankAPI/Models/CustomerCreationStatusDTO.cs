using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class CustomerCreationStatusDTO
    {
        public int Id { get; set; }
       
        public string CustomerId { get; set; }
        
        public string Message { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
