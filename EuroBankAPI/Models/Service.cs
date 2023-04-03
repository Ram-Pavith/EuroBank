using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class Service
    {
        
        public Service() { }
        [Key]
        public int ServiceId { get; set; } 
        public DateTime DateServiceProvided { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}
