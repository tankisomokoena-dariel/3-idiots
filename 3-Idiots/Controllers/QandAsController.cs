using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3_Idiots.Models;

namespace _3_Idiots.Controllers
{
    public class QandAsController : Controller
    {
        private Entities2 db = new Entities2();

        // GET: QandAs
        public ActionResult Index()
        {
            var qandAs = db.QandAs.Include(q => q.userTable);
            return View(qandAs.ToList());
        }

        // GET: QandAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QandA qandA = db.QandAs.Find(id);
            if (qandA == null)
            {
                return HttpNotFound();
            }
            return View(qandA);
        }

        // GET: QandAs/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.userTables, "userID", "firstName");
            return View();
        }

        // POST: QandAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "qaID,userID,answer,question")] QandA qandA)
        {
            if (ModelState.IsValid)
            {
                db.QandAs.Add(qandA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(db.userTables, "userID", "firstName", qandA.userID);
            return View(qandA);
        }

        // GET: QandAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QandA qandA = db.QandAs.Find(id);
            if (qandA == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.userTables, "userID", "firstName", qandA.userID);
            return View(qandA);
        }

        // POST: QandAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "qaID,userID,answer,question")] QandA qandA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qandA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.userTables, "userID", "firstName", qandA.userID);
            return View(qandA);
        }

        // GET: QandAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QandA qandA = db.QandAs.Find(id);
            if (qandA == null)
            {
                return HttpNotFound();
            }
            return View(qandA);
        }

        // POST: QandAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QandA qandA = db.QandAs.Find(id);
            db.QandAs.Remove(qandA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
