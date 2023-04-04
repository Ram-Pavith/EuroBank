using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.DTOs
{
    public class AccountDTO
    {
       /* public AccountDTO()
        {
            TransactionStatuses = new HashSet<TransactionStatusDTO>();
            Statements = new HashSet<StatementDTO>();
            Transactions = new HashSet<TransactionDTO>();
        }*/

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public int AccountTypeId { get; set; }
        [ForeignKey("AccountCreationStatusDTO")]
        public int AccountCreationStatusId { get; set; }
        public string CustomerId { get; set; }

        public DateTime DateCreated { get; set; }

        public double Balance { get; set; }

       /* public virtual AccountTypeDTO? AccountType { get; set; } = null!;

        public virtual CustomerDTO? Customer { get; set; } = null!;

        public virtual AccountCreationStatusDTO? AccountCreationStatus { get; set; }

        public virtual ICollection<StatementDTO>? Statements { get; set; }
        public virtual ICollection<TransactionDTO>? Transactions { get; }

        public virtual ICollection<TransactionStatusDTO>? TransactionStatuses { get; set; }
*/    }
}
