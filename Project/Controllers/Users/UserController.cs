using DbHandler.Db;
using FacebookTools.FacebookObjects;
using DbHandler.Objects;
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
            ViewBag.topHappiest = top;

            Dictionary<string, List<PhotoAndEmotions>> topFriends = new Dictionary<string, List<PhotoAndEmotions>>();
            List<UserPhotosEmotions> topF = new List<UserPhotosEmotions>();
            if (!string.IsNullOrEmpty(person.Token))
            {
                FacebookTools.FacebookHelper helper = new FacebookTools.FacebookHelper(person.Token, socieId);
                var userFriends = helper.GetFriends();
                foreach (var userFriend in userFriends)
                {
                    Person socieUser = driver.getSocieUser(userFriend.PersonId);
                    if (socieUser != null)
                    {
                        var fr = new UserPhotosEmotions();
                        fr.userId = userFriend.PersonId;
                        fr.userName = userFriend.Name;
                        fr.photosEmotions = driver.GetUserPhotosByHappiness(socieUser.SocieId);

                        topF.Add(fr);

                        var key = string.Format("\"{0}\"", userFriend.PersonId);
                        topFriends.Add(key, new List<PhotoAndEmotions>());
                        var topCurrentFriend = driver.GetUserPhotosByHappiness(socieUser.SocieId);

                        topFriends[key] = topCurrentFriend;
                    }
                }
            }

            ViewBag.topFriends = topF;
            return View();
        }
    }
}