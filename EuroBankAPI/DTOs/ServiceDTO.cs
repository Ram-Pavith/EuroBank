using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace EuroBankAPI.DTOs
{
    public class ServiceDTO
    {
       

        [Key]
        public int ServiceId { get; set; }
        public DateTime DateServiceProvided { get; set; }
        public string ServiceName { get; set; }
    }
}
