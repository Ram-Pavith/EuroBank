using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class CustomerCreationStatusDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        //public virtual CustomerDTO Customer { get; set; }

    }
}
