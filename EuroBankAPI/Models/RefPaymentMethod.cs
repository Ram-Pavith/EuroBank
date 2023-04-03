using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefPaymentMethod
    {
        [Key]
        public int PaymentMethodCode { get; set; } 
        public string PaymentMethodName { get; set; }
    }
}
