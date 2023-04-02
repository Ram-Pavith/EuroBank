using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class Services
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; } 
        public DateOnly DateServiceProvided { get; set; }

    }
}
