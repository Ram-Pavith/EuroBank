using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class RefTransactionSatusDTO
    {
        public string TransactionStatusCode { get; set; }
        public string TransactionStatusDescriptions { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
