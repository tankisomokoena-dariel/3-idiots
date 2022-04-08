using _3_Idiots.Models;
using IdiotsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3_Idiots.Controllers
{
    public class QandAController : Controller
    {
        QandAClient QandAClient = new QandAClient();
        // GET: QandA
        public ActionResult Index()
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int userID = Convert.ToInt32(Session["userID"]);
                var myQuestions = QandAClient.ViewMyQuestionsAsync(userID).Result;
                return View(myQuestions.ToList());
            }
        }

        // GET: QandA/Details/5
        public ActionResult Details(int id)
        {
            var entry = QandAClient.GetAsync(id).Result;
            return View(entry);
        }

        // GET: QandA/Create
        public ActionResult Create()
        {
            if (Session["userID"] != null)
            {
                var result = QandAClient.UnuansweredQuestionsAsync().Result;
                return View(result.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: QandA/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: QandA/Edit/5
        public ActionResult Edit(int id)
        {
            var entry = QandAClient.GetAsync(id).Result;
            return View(entry);
        }

        // GET: QandA/Edit/5
        public ActionResult EditQuestion(int id)
        {
            var entry = QandAClient.GetAsync(id).Result;
            return View(entry);
        }

        // POST: QandA/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "qaID, userID, answer, question")] QandA update)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    QandAClient.UpdateMyQuestionOrAnswerAsync(update.UserID, (int)update.QaID, update).Wait();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        // POST: QandA/Edit/5
        [HttpPost]
        public ActionResult EditQuestion([Bind(Include = "qaID, userID, answer, question")] QandA update)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    QandAClient.UpdateMyQuestionOrAnswerAsync(update.UserID, (int)update.QaID, update).Wait();
                }
                return RedirectToAction("Create", "QandA");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        // GET: QandA/Delete/5
        public ActionResult Delete(int id)
        {
            var entry = QandAClient.GetAsync(id).Result;
            return View(entry);
        }

        // POST: QandA/Delete/5
        [HttpPost]
        public ActionResult Delete([Bind(Include = "qaID, userID, answer, question")] QandA delete)
        {
            try
            {
                // TODO: Add delete logic here
                QandAClient.DeleteAsync((int)delete.QaID).Wait();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Get: QandA/Search/searchString
        [HttpGet]
        public ActionResult Search()
        {
            if (Session["userID"] != null)
            {
                var search = HttpContext.Request.QueryString["search"];
                if (!string.IsNullOrWhiteSpace(search))
                {
                    int userID = Convert.ToInt32(Session["userID"]);
                    var response = QandAClient.SearchAsync(search, userID).Result;
                    return View(response);
                }
                else
                {
                    ViewBag.ResponseMessage = "Cannot search an empty input";
                    return View("~/Views/Home/Home.cshtml");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Navigate()
        {

            //var currentUrl = this.Request.RawUrl;

            if (Session["userID"] != null)
            {
                var command = HttpContext.Request.QueryString["search"];
                var currentUrl = HttpContext.Request.QueryString["currentUrl"];

                if (command.ToLower().Contains("information hub") || command.ToLower().Contains("home"))
                {

                    return RedirectToAction("Home", "Home");
                }
                else if (command.ToLower().Contains("unanswered"))
                {
                    return RedirectToAction("Create");
                }
                else if (command.ToLower().Contains("question") || command.ToLower().Contains("questions"))
                {
                    return RedirectToAction("Index");
                }
                else if (command.ToLower().Contains("skill") || command.ToLower().Contains("skills"))
                {
                    return RedirectToAction("GetUserSkills", "Skills");
                }
                else
                {
                    //Response.Redirect(Request.RawUrl);
                    return Redirect(currentUrl);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            if (Session["userID"] != null)
            {
                if (string.IsNullOrEmpty(search))
                {
                    ViewBag.ResponseMessage = "Cannot search an empty input";
                    return View("~/Views/Home/Home.cshtml");
                }
                else
                {
                    int userID = Convert.ToInt32(Session["userID"]);
                    var response = QandAClient.SearchAsync(search, userID).Result;
                    return View(response);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

    }
}
