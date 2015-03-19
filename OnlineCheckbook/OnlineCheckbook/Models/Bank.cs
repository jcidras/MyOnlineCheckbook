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

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public double TotalWithdrawls { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public double TotalDeposits { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public double CurrentBalance { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
