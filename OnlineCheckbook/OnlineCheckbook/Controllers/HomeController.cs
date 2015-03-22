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

        [ActionName("Profile")]
        public ActionResult UserProfile(int? id)
        {
            if (id == null)
            {
                return View("ErrorPage");
            }
            var user = db.Users.Find(id);
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include="Username,Password")] User user)
        {
            try
            {
                var tempUser = db.Users.Where(u => u.Username == user.Username && u.Password == user.Password).Single();
                Session["UserId"] = tempUser.UserID;
                return RedirectToAction("Profile", new { id = tempUser.UserID });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Either username or password is incorrect. Please try again.");                
            }
            return View(user);
        }
    }
}