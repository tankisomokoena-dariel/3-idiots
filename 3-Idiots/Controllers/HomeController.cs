using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3_Idiots.Controllers
{
	public class HomeController : Controller
	{
		userTablesController dbAccess = new userTablesController();
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
			if(dbAccess.VerifyDetails(email,password) == -1)
            {
				return View("~/Views/Home/Index.cshtml");
            }
            else
            {
				return View("~/Views/Home/Home.cshtml");
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