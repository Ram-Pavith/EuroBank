using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class Service
    {
      
        
        [Key]
        public int ServiceId { get; set; } 
        public string ServiceName { get; set; }
        public DateTime DateServiceProvided { get; set; }

    }
}
