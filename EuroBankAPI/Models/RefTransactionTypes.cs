﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class RefTransactionTypes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionTypeCode { get; set; } 
        public string TransactionTypeDescriptions { get; set; }

    }
}
