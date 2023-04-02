

using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models

{
    public class Account
    {
        public Account()
        {

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public double Balance { get; set; }

        public virtual AccountType AccountType { get; set; }

        //public virtual Customer CustomerId { get; set; }

    }
}
