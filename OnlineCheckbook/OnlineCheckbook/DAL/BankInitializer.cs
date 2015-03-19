using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using OnlineCheckbook.Models;

namespace OnlineCheckbook.DAL
{
    class BankInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            var users = new List<User>
            {
                new User {UserID=1, Username="jtcidras", Password="123",},
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var banks = new List<Bank>
            {
                new Bank {BankID=1, UserID=1, BankName="First National Bank of Leavenworth"},
                new Bank {BankID=2, UserID=1, BankName="Horizon Credit Union"}
            };

            banks.ForEach(b => context.Banks.Add(b));
            context.SaveChanges();

            var expenses = new List<Expense> 
            { 
                new Expense {BankID=1, ExpenseID=1, Amount=12.34, Date=DateTime.Parse("2015-01-01"), Description="Buying Pizza"}
            };

            expenses.ForEach(e => context.Expenses.Add(e));
            context.SaveChanges();
        }
    }
}
