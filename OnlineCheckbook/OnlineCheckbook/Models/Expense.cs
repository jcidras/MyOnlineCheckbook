using System;

namespace OnlineCheckbook.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }

        public int BankID { get; set; }

        public double ExpenseAmount { get; set; }

        public string ExpenseDescription { get; set; }

        public DateTime ExpenseDate { get; set; }
    }
}
