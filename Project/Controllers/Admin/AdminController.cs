using Project.Models;
using Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            getStatistics();
            return View();
        }

        public ActionResult GetAjaxDiv()
        {

            return PartialView("AjaxDiv");
        }

        private void getStatistics()
        {
            var db = new ApplicationDbContext();
            var stats = from st in db.StatisticsPerDay
                        select new
                        {
                            Amount = st.Amount,
                            Date = st.Date
                        };
            var c = stats.ToList();
            List<SaleStat> dict = new List<SaleStat>();
            foreach (var v in c)
            {
                SaleStat s = new SaleStat()
                {
                    date = v.Date,
                    total = v.Amount
                };
                dict.Add(s);
            }

            ViewBag.StatsJson = dict;
            //Dictionary<string, float> statisticsInfo = new Dictionary<string, float>();
            //foreach (var stat in c)
            //{
            //    stat
            //}


            ViewBag.Stats = c;
            return;
        }
    }
}