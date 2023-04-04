using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefPaymentMethod
    {
        public RefPaymentMethod()
        {
            Transactions = new HashSet<Transaction>();
        }
        [Key]
        public int PaymentMethodCode { get; set; } 
        public string PaymentMethodName { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
