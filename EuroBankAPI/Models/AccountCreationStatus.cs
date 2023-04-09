using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class AccountCreationStatus
    {
        [Key]
        public int AccountCreationStatusId { get; set; }
        public string Message { get; set; } = string.Empty;


    }
}
