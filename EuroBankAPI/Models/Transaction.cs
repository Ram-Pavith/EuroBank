﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroBankAPI.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        [Required]
        public Guid CounterPartyId { get; set; }
        public Guid AccountId { get; set; }= Guid.NewGuid();
        public int ServiceId { get; set; }
        public int RefTransactionStatusId { get; set; }
        public int RefTransactionTypeId { get; set; }
        public int RefPaymentMethodId { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public double AmountOfTransaction { get; set; }
        public virtual CounterParty? CounterParty { get; set; }
        public virtual Service? Service { get; set; }
        public virtual RefTransactionStatus? RefTransactionStatus { get; set; }
        public virtual RefTransactionType? RefTransactionType { get; set; }
        public virtual RefPaymentMethod? RefPaymentMethod { get; set; }
        public virtual Account? Account { get; set; }



    }
}
