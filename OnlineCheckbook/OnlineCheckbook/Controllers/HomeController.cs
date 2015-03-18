using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineCheckbook.Models;
using System.Data.Entity;
using System.Data;
using OnlineCheckbook.DAL;
using System.Net;
namespace OnlineCheckbook.Controllers
{
    public class HomeController : Controller
    {
        private BankContext db = new BankContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Account(int? id)
        {
            if (id == null)
            {
                return View("ErrorPage");
            }
            var user = db.Users.Find(id);
            return View(user);
        }

        /// <summary>
        /// This method is by coming back from a different controller
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AccountPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include="Username,Password")] User user)
        {     
            var tempUser = db.Users.Where(u => u.Username == user.Username 
                && u.Password == user.Password).Single();        
            if (tempUser == null)
            {
                ModelState.AddModelError("", "Either username or password is incorrect. Please try again.");
                return View(user);
            }
            return RedirectToAction("Account", new { id = tempUser.UserID });
        }
    }
}