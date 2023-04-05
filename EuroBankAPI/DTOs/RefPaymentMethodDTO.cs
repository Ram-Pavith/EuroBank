using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class RefPaymentMethodDTO
    {
        public RefPaymentMethodDTO() {
            Transactions = new HashSet<TransactionDTO>();
        }
        [Key]
        public int PaymentMethodCode { get; set; }
        public string PaymentMethodName { get; set; }
        public virtual ICollection<TransactionDTO> Transactions { get; set; }
    }
}
