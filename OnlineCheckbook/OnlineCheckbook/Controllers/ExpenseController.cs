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
        public ActionResult Create(int? id)
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
        public ActionResult Create([Bind(Include="BankID, Amount, Description, Date")] Expense expense)
        {
            try
            {
                db.Expenses.Add(expense);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return View();
        }
    }
}