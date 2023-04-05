using System.ComponentModel.DataAnnotations;

namespace EuroBankAPI.Models
{
    public class RuleStatus
    {
        [Key]
        public int RuleStatusId { get; set; }
        public string Status;
    }
}
