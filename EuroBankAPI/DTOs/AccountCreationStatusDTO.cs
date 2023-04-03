using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class AccountCreationStatusDTO
    {
        [Key]
        public int AccountCreationStatusId { get; set; }

        /*  [ForeignKey("Account")]
          public Guid AccountId { get; set; }*/
        public string Message { get; set; } = string.Empty;

        public virtual AccountDTO Account { get; set; } = null!;
    }
}
