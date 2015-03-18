using System;
using System.Collections.Generic;

namespace OnlineCheckbook.Models
{
    public class Bank
    {
        public int BankID { get; set; }

        public int UserID { get; set; }

        public string BankName { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
