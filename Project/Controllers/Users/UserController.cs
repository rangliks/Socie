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
            Person person = driver.GetPerson(socieId);
            ViewBag.personId = person.PersonId;
            var top = driver.GetUserPhotosByHappiness(socieId);
            ViewBag.topHappiest = top.Keys;
            ViewBag.topHappiestScores = top.Values;

            if(!string.IsNullOrEmpty(person.Token))
            {
                FacebookTools.FacebookHelper helper = new FacebookTools.FacebookHelper(person.Token, socieId);
                var userFriends = helper.GetFriends();
            }
            
            return View();
        }
    }
}