using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdiotsAPI;
using _3_Idiots.Models;

namespace _3_Idiots.Controllers
{
    public class HomeController : Controller
    {
        UsersClient client = new UsersClient();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Login(string email, string password)
        {
            Authenticate user = new Authenticate();
            user.Email = email;
            user.Password = password;
            var response = client.LoginAsync(user).Result;
            if (response != 0)
            {
                Session["userID"] = response;
                return View("~/Views/Home/Home.cshtml");
            }
            else
            {
                ViewBag.ResponseMessage = "Incorrect email or password";
                return View("~/Views/Home/Index.cshtml");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}