using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineCheckbook.Models;
using OnlineCheckbook.DAL;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace OnlineCheckbook.Controllers
{
    public class BankController : Controller
    {
        private BankContext db = new BankContext();
        // GET: Bank
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            return View(bank);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = new Bank();
            bank.UserID = id;
            return View(bank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="BankName, UserID")] Bank bank)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Banks.Add(bank);
                    db.SaveChanges();
                    return RedirectToAction("Profile", "Home", new { id = bank.UserID });
                }
            }
            catch (Exception)
            {
            }
            return View(bank);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                View("ErrorPage");
            }
            var bank = db.Banks.Find(id);
            return View(bank);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int bankID)
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
            return RedirectToAction("Profile", "Home", new { id = bankToUpdate.UserID });
        }

        public ActionResult Delete(int id, int userId)
        {
            try
            {
                Bank bankToBeDeleted = new Bank() { BankID = id };
                db.Entry(bankToBeDeleted).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to delete bank. Please try again later.");
            }
            return RedirectToAction("Profile", "Home", new { id = userId });
        }
    }
}