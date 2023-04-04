using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models

{
    public class Account
    {     
        public Account() 
        { 
            TransactionStatuses = new HashSet<TransactionStatus>();
            Transactions = new HashSet<Transaction>();
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public int AccountTypeId { get; set; }
        [ForeignKey("AccountCreationStatus")]
        public int AccountCreationStatusId { get; set; }
        public string CustomerId { get; set; } 

        public DateTime DateCreated { get; set; }

        public double Balance { get; set; }

        public virtual AccountType AccountType { get; set; } = null!;
        
        public virtual Customer Customer { get; set; } = null!;

        public virtual AccountCreationStatus AccountCreationStatus { get; set; }

        public virtual ICollection<Statement> Statements { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<TransactionStatus> TransactionStatuses { get; set; }
    }
}
