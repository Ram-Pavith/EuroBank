using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class AccountCreationStatus
    {
        [Key]
        public int AccountCreationStatusId { get; set; }

      /*  [ForeignKey("Account")]
        public Guid AccountId { get; set; }*/
        public string Message { get; set; } = string.Empty;

/*        public virtual Account Account { get; set; } = null!;
*/
    }
}
