using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineCheckbook.Models
{
    public class Bank
    {
        public int? BankID { get; set; }

        public int? UserID { get; set; }

        public string BankName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
