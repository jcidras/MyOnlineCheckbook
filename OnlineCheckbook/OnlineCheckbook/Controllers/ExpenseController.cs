using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using OnlineCheckbook.Models;
using OnlineCheckbook.DAL;

namespace OnlineCheckbook.Controllers
{
    public class ExpenseController : Controller
    {
        BankContext db = new BankContext();
        // GET: Expense
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Takes the Bank ID and creates a new Expense with that ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Withdrawl(int? id)
        {
            if (id == null)
            {
                View("ErrorPage");
            }
            var expense = new Expense() { BankID = id };
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdrawl([Bind(Include="BankID, Amount, Description, Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bank = db.Banks.Find(expense.BankID);
                    bank.AddWithdrawl(expense);
                    db.Expenses.Add(expense);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return View(expense);
                }
            }
            return RedirectToAction("Index", "Bank", new { id = expense.BankID });
        }

        /// <summary>
        /// Takes the Bank ID and creates a new Expense with that ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Deposit(int? id)
        {
            if (id == null)
            {
                View("ErrorPage");
            }
            var expense = new Expense() { BankID = id };
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Include = "BankID, Amount, Description, Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bank = db.Banks.Find(expense.BankID);
                    bank.AddDeposit(expense);
                    db.Expenses.Add(expense);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return View(expense);
                }
            }
            return RedirectToAction("Index", "Bank", new {id = expense.BankID});
        }
    }
}