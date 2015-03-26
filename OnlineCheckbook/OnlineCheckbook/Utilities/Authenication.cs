using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineCheckbook.Models;
using OnlineCheckbook.DAL;

namespace OnlineCheckbook.Utilities
{
    public class Authenication
    {
        private static BankContext db = new BankContext();

        public static bool IsAuthorized(Object model, Object userId)
        {
            bool isAuthorized = false;
            if (userId != null && model != null)
            {
                var id = Int32.Parse(userId.ToString());                
                if (model is Account)
                {
                    var account = (Account)model;
                    if (0 < (from banks in db.Banks 
                             join accounts in db.Accounts on banks.BankID equals account.BankId
                             where banks.UserID == id && account.Id == account.Id
                             select accounts).AsEnumerable().ToList().Count())
                    {
                        isAuthorized = true;
                    }
                }
                else if (model is Bank)
                {
                    var bank = (Bank)model;
                    if (0 < (from users in db.Users
                             join banks in db.Banks on users.UserID equals banks.UserID
                             where users.UserID == id && banks.BankID == bank.BankID
                             select banks).AsEnumerable().ToList().Count())
                    {
                        isAuthorized = true;
                    }
                }
                else
                {
                    var expense = (Expense)model;
                    if (0 < (from banks in db.Banks
                             join accounts in db.Accounts on banks.BankID equals accounts.BankId
                             join expenses in db.Expenses on accounts.Id equals expenses.AccountID
                             where banks.UserID == id && expenses.ExpenseID == expense.ExpenseID
                             select expenses).AsEnumerable().ToList().Count())
                    {
                        isAuthorized = true;
                    }
                }
            }
            return isAuthorized;
        }
    }
}