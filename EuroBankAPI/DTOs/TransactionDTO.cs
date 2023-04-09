using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.DTOs
{
    public class TransactionDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        [Required]
        public string CounterPartyId { get; set; }
        public Guid AccountId { get; set; } = Guid.NewGuid();
        public int ServiceId { get; set; }
        public int RefTransactionStatusId { get; set; }
        public int RefTransactionTypeId { get; set; }
        public int RefPaymentMethodId { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public double AmountOfTransaction { get; set; }
    }
}
