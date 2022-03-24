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
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Login(string email, string password)
        {
            Authenticate user = new Authenticate();
            user.Email = email;
            user.Password = password;
            var response = client.LoginAsync(user).Result;
            if (response == -3)
            {
                ViewBag.ResponseMessage = "Server error!";
                return View("~/Views/Home/Index.cshtml");
            }
            else if (response == -2)
            {
                ViewBag.ResponseMessage = "Incorrect email or password";
                return View("~/Views/Home/Index.cshtml");
            }else if(response == -1)
            {
                return View("~/Views/Home/ChangePassword.cshtml");
            }
            else
            {
                Session["userID"] = response;
                return RedirectToAction("Home","Home");
            }
        }

        public ActionResult ChangePassword(Authenticate user)
        {
            var isChanged = client.ChangeDefaultPasswordAsync(user).Result;
            if (isChanged)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult About()
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
        }

        public ActionResult Contact()
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Your contact page.";
                return View();
            }
        }
    }
}