using _3_Idiots.Models;
using IdiotsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
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
            CheckSession();
            int userID = Convert.ToInt32(Session["userID"]);
            var myQuestions = QandAClient.ViewMyQuestionsAsync(userID).Result;
            return View(myQuestions.ToList());
        }

        // GET: QandA/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QandA/Create
        public ActionResult Create()
        {
            var result = QandAClient.UnuansweredQuestionsAsync().Result;
            return View(result.ToList());
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
            CheckSession();
            var entry = QandAClient.GetAsync(id).Result;
            return View(entry);
        }

        // POST: QandA/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "qaID, userID, answer, question")] QandA update)
        {
            CheckSession();
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    QandAClient.UpdateMyQuestionOrAnswerAsync(update.UserID, (int)update.QaID, update).Wait();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: QandA/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QandA/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Get: QandA/Search/searchString
        [HttpPost]
        public ActionResult Search(string search)
        {
            CheckSession();
            int userID = Convert.ToInt32(Session["userID"]);
            var response = QandAClient.SearchAsync(search, userID).Result;
            return View(response);

        }

        protected void CheckSession()
        {
            if (Session["userID"] == null)
            {
                RedirectToAction("Index", "Home");
            }
        }
    }
}
