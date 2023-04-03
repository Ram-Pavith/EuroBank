using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefTransactionType
    {
        public RefTransactionType() { }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // if this is a predefined table then why the above annotation should be given?
        public int TransactionTypeCode { get; set; } 
        public string TransactionTypeDescriptions { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}
