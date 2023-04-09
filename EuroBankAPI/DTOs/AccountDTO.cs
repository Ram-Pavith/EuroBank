using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.DTOs
{
    public class AccountDTO
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public int AccountTypeId { get; set; }
        [ForeignKey("AccountCreationStatusDTO")]
        public int AccountCreationStatusId { get; set; }
        public string CustomerId { get; set; }

        public DateTime DateCreated { get; set; }

        public double Balance { get; set; }

       
    }
}
