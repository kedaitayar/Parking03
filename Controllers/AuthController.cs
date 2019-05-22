using Parking03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Parking03.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            Database1Entities db = new Database1Entities();
            int count = (from x in db.Users
                         where x.username == user.username
                         where x.pass == user.pass
                         select x).Count();
            if (count == 0)
            {
                ViewBag.errMsg = "No match found";
                return View();
            }
            else
            {
                var id = (from x in db.Users
                          where x.username == user.username
                          where x.pass == user.pass
                          select x.Id).FirstOrDefault();
                Session["Username"] = user.username;
                Session["ID"] = id;
                FormsAuthentication.SetAuthCookie(user.username, false);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            //signout
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }


        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
    }
}