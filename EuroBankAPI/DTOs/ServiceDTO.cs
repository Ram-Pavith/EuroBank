using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class ServiceDTO
    {
        [Key]
        public int ServiceId { get; set; }
        public DateTime DateServiceProvided { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
