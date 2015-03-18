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
    public class UserController : Controller
    {
        private BankContext db = new BankContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // Create user
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Username, Password")]User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return View();
            }
            return RedirectToAction("Account", "Home", 
                new { 
                        id = db.Users.Where(u => u.Username == user.Username 
                                && u.Password == user.Password).Single().UserID 
                });
        }
    }
}