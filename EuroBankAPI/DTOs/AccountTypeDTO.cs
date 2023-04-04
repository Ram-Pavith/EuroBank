using EuroBankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.DTOs
{
    public class AccountTypeDTO
    {
        public AccountTypeDTO()
        {
            Accounts = new HashSet<AccountDTO>();
        }
        [Key]
        public int AccountTypeId { get; set; }
        public string Type { get; set; } = string.Empty;
        public virtual ICollection<AccountDTO>? Accounts { get; set; }
    }
}
