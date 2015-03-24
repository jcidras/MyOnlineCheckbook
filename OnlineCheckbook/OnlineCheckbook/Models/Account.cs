using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineCheckbook.Models
{
    public class Account
    {
        public int Id { get; set; }

        public int? BankId { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public double TotalWithdraws { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public double TotalDeposits { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public double CurrentBalance { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public void AddDeposit(Expense expense)
        {
            this.CurrentBalance += expense.Amount;
            this.TotalDeposits += expense.Amount;
        }

        public void AddWithdraw(Expense expense)
        {
            this.CurrentBalance -= expense.Amount;
            this.TotalWithdraws -= expense.Amount;
        }
    }
}