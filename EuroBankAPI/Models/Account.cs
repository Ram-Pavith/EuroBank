using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models

{
    public class Account
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public int AccountTypeId { get; set; }

        public string CustomerId { get; set; } 

        public DateTime DateCreated { get; set; };

        public double Balance { get; set; }

        public virtual AccountType AccountType { get; set; } = null!;

        //public virtual Customer Customer { get; set; } = null!;

    }
}
