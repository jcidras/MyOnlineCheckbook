using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using OnlineCheckbook.DAL;
using OnlineCheckbook.Models;

namespace OnlineCheckbook.Controllers
{
    public class AccountController : Controller
    {
        BankContext db = new BankContext();
        // GET: Account
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return View("ErrorPage");
            }
            Account account = db.Accounts.Find(id);
            return View(account);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View("ErrorPage");
            }
            Account account = new Account() { BankId = id };
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankId, Name, CurrentBalance")]Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Bank", new { id = account.BankId });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to create account. Please try again later.");
                }
            }
            return View(account);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                View("ErrorPage");
            }
            Account account = db.Accounts.Find(id);
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name")] int id)
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
            return RedirectToAction("Index", "Bank", new { id = accountToUpdate.BankId });
        }

        public ActionResult Delete(int id, int bankId)
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
            return RedirectToAction("Index", "Bank", new { id = bankId });
        }
    }
}