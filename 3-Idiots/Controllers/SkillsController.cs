using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdiotsAPI;

namespace _3_Idiots.Controllers
{
    public class SkillsController : Controller
    {
        SkillsClient client = new SkillsClient();
        // GET: Skills
        public ActionResult Index()
        {
            if (Session["userID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult GetAllSkills()
        {
            if (Session["userID"] != null)
            {
                var result = client.GetAllSkillsAsync().Result;
                return View(result.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetUserSkills()
        {
            if (Session["userID"] != null)
            {
                int userID = Convert.ToInt32(Session["userID"]);
                var result = client.GetUserSkillsAsync(userID).Result;
                return View(result.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Edit(int userID, string skillDescription)
        {
            var result = client.GetUserSkillsAsync(userID).Result;
            var skill = result.Where(s => s.Descriprion == skillDescription).FirstOrDefault();
            return View(skill);
        }

        [HttpPost]
        public ActionResult UpdateSkill(UserSkill skill)
        {
            client.PutAsync(skill).Wait();
            return RedirectToAction("GetUserSkills");
        }
    }
}