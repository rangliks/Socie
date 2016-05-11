using DbHandler.Db;
using FacebookTools.FacebookObjects;
using DbHandler.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OxfordTools;

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
            ViewBag.allPhotos = driver.GetUserPhotos(socieId);

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
                        var userPhotos = driver.GetUserPhotosByEmotion(socieUser.SocieId, Emotion.Happiness, 5);
                        var fr = new UserPhotosEmotions
                        {
                            userId = userFriend.PersonId,
                            userName = userFriend.Name,
                            photosEmotions = userPhotos
                        };
                        topF.Add(fr);

                        var key = string.Format("\"{0}\"", userFriend.PersonId);
                        topFriends.Add(key, new List<PhotoAndEmotions>());
                        topFriends[key] = userPhotos;
                    }
                }
            }

            ViewBag.topFriends = topF;
            return View();
        }
    }
}