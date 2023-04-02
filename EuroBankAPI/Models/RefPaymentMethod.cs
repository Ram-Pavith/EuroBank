using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefPaymentMethod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodCode { get; set; } 
        public string PaymentMethodName { get; set; }
    }
}
