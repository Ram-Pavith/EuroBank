using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; } 
        public DateOnly DateServiceProvided { get; set; }

    }
}
