using DbHandler.Db;
using FacebookTools.FacebookObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Project.Controllers.Users
{
    public class UserController : Controller
    {
        DbDriver driver = new DbDriver();
        // GET: User
        public ActionResult Index()
        {
            var socieId = User.Identity.GetUserId();
            ViewBag.personId = driver.getPersonId(socieId);
            ViewBag.topHappiest = driver.GetUserPhotosByHappiness(socieId);

            return View();
        }
    }
}