using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class RefPaymentMethodDTO
    {
        [Key]
        public int PaymentMethodCode { get; set; }
        public string PaymentMethodName { get; set; }
    }
}
