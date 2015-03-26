using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using OnlineCheckbook.DAL;
using OnlineCheckbook.Models;
using OnlineCheckbook.Utilities;

namespace OnlineCheckbook.Controllers
{     
    public class UserController : Controller
    {
        private BankContext db = new BankContext();
               
        #region User
        // GET: User 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserDetails()
        {
            var sessionUserId = Session[SessionVariable.USER_ID];
            if (sessionUserId != null)
            {
                var id = Int32.Parse(sessionUserId.ToString());
                var user = db.Users.Find(id);
                return View(user);
            }
            return View("Login");
        }

        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin([Bind(Include = "Username,Password")] User user)
        {
            try
            {                
                var tempUser = db.Users.Where(u => u.Username == user.Username && u.Password == user.Password).Single();
                Session[SessionVariable.USER_ID] = tempUser.UserID;
                return RedirectToAction("UserDetails");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Either username or password is incorrect. Please try again.");
            }
            return View(user);
        }
        
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "Username, Password")]User user)
        {
            try
            {
                if (0 == db.Users.Select(x => x.Username == user.Username).Count())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["UserId"] = db.Users.Where(x => x.Username == user.Username &&
                                                                           x.Password == user.Password).Single().UserID;
                }
                else
                {
                    ModelState.AddModelError("", "Username is already taken.");
                    return View(user);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to create profile. Try again later.");
                return View(user);
            }
            return RedirectToAction("UserDetails");
        }

        public ActionResult EditUser()
        {
            var id = Session[SessionVariable.USER_ID];
            if (id == null)
            {
                return View("ErrorPage");
            }
            var user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserPost(User user)
        { 
            try
            {
                var userToBeUpdated = db.Users.Find(user.UserID);
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (TryUpdateModel(userToBeUpdated, "", new string[] { "Password" }))
                    {
                        db.SaveChanges();
                    }                    
                }
                if (!string.IsNullOrEmpty(user.Username))
                {
                    if (TryUpdateModel(userToBeUpdated, "", new string[] { "Username" }))
                    {
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to update profile. Please try again later.");
            }
            return RedirectToAction("UserDetails");
        }

        [HttpGet]
        public ActionResult DeleteUser()
        {
            try
            {
                var id = Session[SessionVariable.USER_ID];
                if (id == null)
                {
                    return View("ErrorPage");
                }
                User user = new User() { UserID = Int32.Parse(id.ToString()) };
                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to delete profile at this time. Please try again later.");
                return RedirectToAction("UserDetails");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session[SessionVariable.USER_ID] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Bank
        public ActionResult BankDetails(int id)
        {
            Bank bank = db.Banks.Find(id);            
            if (bank == null || !Authenication.IsAuthorized(bank, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }
            return View(bank);
        }

        public ActionResult AddBank()
        {
            var id = Session[SessionVariable.USER_ID];
            if (id == null)
            {
                return View("ErrorPage");
            }
            Bank bank = new Bank() { UserID = Int32.Parse(id.ToString()) };
            return View(bank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBank([Bind(Include = "BankName, UserID")] Bank bank)
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Banks.Add(bank);
                    db.SaveChanges();
                    return RedirectToAction("UserDetails");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to create a bank. Please try again later.");
            }
            return View(bank);
        }

        public ActionResult EditBank(int? id)
        {
            var bank = db.Banks.Find(id);
            if (!Authenication.IsAuthorized(bank, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }            
            return View(bank);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditBankPost(int bankID)
        {
            var bankToUpdate = db.Banks.Find(bankID);
            if (TryUpdateModel(bankToUpdate, "",
                new string[] { "BankName" }))
            {
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to update your bank. Please try again later.");
                    return View(bankToUpdate);
                }
            }
            return RedirectToAction("UserDetails");
        }

        public ActionResult DeleteBank(int id)
        {
            var bank = db.Banks.Find(id);
            if (!Authenication.IsAuthorized(bank, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }
            try
            {
                db.Banks.Remove(bank);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to delete bank. Please try again later.");
            }
            return RedirectToAction("UserDetails");
        }        
        #endregion

        #region Account
        // GET: Account
        public ActionResult AccountDetails(int? id)
        {
            var account = db.Accounts.Find(id);
            if (account == null || !Authenication.IsAuthorized(account, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }            
            return View(account);
        }

        [HttpGet]
        public ActionResult AddAccount(int? id)
        {
            var bank = db.Banks.Find(id);
            if (bank == null || !Authenication.IsAuthorized(bank, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }
            Account account = new Account() { BankId = id };
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccount([Bind(Include = "BankId, Name, CurrentBalance")]Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("BankDetails", new { id = account.BankId });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to create account. Please try again later.");
                }
            }
            return View(account);
        }

        public ActionResult EditAccount(int? id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null || !Authenication.IsAuthorized(account, Session[SessionVariable.USER_ID]))
            {
                View("ErrorPage");
            }
            
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccountPost([Bind(Include = "Name")] int id)
        {
            Account accountToUpdate = db.Accounts.Find(id);
            if (TryUpdateModel(accountToUpdate, "", new string[] { "Name" }))
            {
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to update your account. Please try again later.");
                    return View(accountToUpdate);
                }
            }
            return RedirectToAction("BankDetails");
        }

        public ActionResult DeleteAccount(int id, int bankId)
        {
            var account = db.Accounts.Find(id);
            if (account == null || !Authenication.IsAuthorized(account, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }
            else
            {
                try
                {
                    Account accountToBeDeleted = new Account() { Id = id };
                    db.Entry(accountToBeDeleted).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to delete account. Please try again later.");
                }
            }            
            return RedirectToAction("BankDetails");
        }        
        #endregion

        #region Expense
        // GET: Expense
        public ActionResult ExpenseDetails(int? id)
        {
            var expense = db.Expenses.Find(id);
            if (expense == null || !Authenication.IsAuthorized(expense, Session[SessionVariable.USER_ID]))
            {
                return View("ErrorPage");
            }
            return View(expense);
        }

        /// <summary>
        /// Takes the Bank ID and creates a new Expense with that ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Withdrawl(int? id)
        {
            var account = db.Accounts.Find(id);
            if (account == null || !Authenication.IsAuthorized(account, Session[SessionVariable.USER_ID]))
            {
                View("ErrorPage");
            }
            return View(new Expense() { AccountID = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdrawl([Bind(Include = "AccountID, Amount, Description, Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = db.Accounts.Find(expense.AccountID);
                    account.AddWithdraw(expense);
                    db.Expenses.Add(expense);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return View(expense);
                }
            }
            return RedirectToAction("BankDetails");
        }

        /// <summary>
        /// Takes the Bank ID and creates a new Expense with that ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Deposit(int? id)
        {
            var account = db.Accounts.Find(id);
            if (account == null || !Authenication.IsAuthorized(account, Session[SessionVariable.USER_ID]))
            {
                View("ErrorPage");
            }
            var expense = new Expense() { AccountID = id };
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Include = "AccountID, Amount, Description, Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = db.Accounts.Find(expense.AccountID);
                    account.AddDeposit(expense);
                    db.Expenses.Add(expense);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return View(expense);
                }
            }
            return RedirectToAction("BankDetails");
        }  
        #endregion
    }
}