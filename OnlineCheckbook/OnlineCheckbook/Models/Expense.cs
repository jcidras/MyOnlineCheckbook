using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineCheckbook.Models
{
    public class Expense
    {
        public int? ExpenseID { get; set; }

        public int? BankID { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
