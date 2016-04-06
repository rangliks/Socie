using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.Models.Store;

namespace Project.Controllers.Admin
{
    public class SalesStatsPerDaysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SalesStatsPerDays
        public ActionResult Index()
        {
            return View();
        }

        // GET: SalesStatsPerDays/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //SalesStatsPerDay salesStatsPerDay = db.StatisticsPerDay.Find(id);
            //if (salesStatsPerDay == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: SalesStatsPerDays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesStatsPerDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Amount,Date")] SalesStatsPerDay salesStatsPerDay)
        {
            //if (ModelState.IsValid)
            //{
            //    db.StatisticsPerDay.Add(salesStatsPerDay);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            return View();
        }

        // GET: SalesStatsPerDays/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //SalesStatsPerDay salesStatsPerDay = db.StatisticsPerDay.Find(id);
            //if (salesStatsPerDay == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: SalesStatsPerDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Amount,Date")] SalesStatsPerDay salesStatsPerDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesStatsPerDay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesStatsPerDay);
        }

        // GET: SalesStatsPerDays/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //SalesStatsPerDay salesStatsPerDay = db.StatisticsPerDay.Find(id);
            //if (salesStatsPerDay == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: SalesStatsPerDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //SalesStatsPerDay salesStatsPerDay = db.StatisticsPerDay.Find(id);
            //db.StatisticsPerDay.Remove(salesStatsPerDay);
            //db.SaveChanges();
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
