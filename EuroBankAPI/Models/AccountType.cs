﻿namespace EuroBankAPI.Models
{
    public class AccountType
    {
        public AccountType() 
        { 
            Accounts = new HashSet<Account>();
        }
        public int AccountTypeId { get; set; }
        public string Type { get; set; } = string.Empty;
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
