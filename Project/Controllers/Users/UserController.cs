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
using Newtonsoft.Json;

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

            if (!string.IsNullOrEmpty(person.Token))
            {
                FacebookTools.FacebookHelper helper = new FacebookTools.FacebookHelper(person.Token, socieId);
                ViewBag.userFriends = helper.GetFriends(false);
            }

            return View();
        }

        [HttpGet]
        public string GetSessionData()
        {
            var socieId = User.Identity.GetUserId();
            Person person = driver.GetPerson(socieId);
            ViewBag.personId = person.PersonId;

            if (!string.IsNullOrEmpty(person.Token))
            {
                FacebookTools.FacebookHelper helper = new FacebookTools.FacebookHelper(person.Token, socieId);

                // get profilepicId for session user
                UserPhotosEmotions sessionUserEmotions = driver.GetUserPhotos(socieId);
                var profilePicId = helper.GetProfilePictureId(person);

                return JsonConvert.SerializeObject(sessionUserEmotions);
            }

            return "{}";
        }

        [HttpGet]
        public string GetData()
        {
            var socieId = User.Identity.GetUserId();
            Person person = driver.GetPerson(socieId);

            List<UserPhotosEmotions> topF = new List<UserPhotosEmotions>();
            if (!string.IsNullOrEmpty(person.Token))
            {
                FacebookTools.FacebookHelper helper = new FacebookTools.FacebookHelper(person.Token, socieId);

                var userFriends = helper.GetFriends(false);
                /*/ TODO : Get imaginary friends /*/

                foreach (var userFriend in userFriends)
                {
                    Person socieUser = driver.getSocieUser(userFriend.PersonId);
                    /*/ TODO : Handle imaginary friends /*/
                    if (socieUser != null)
                    {
                        var friendPhotosEmotions = driver.GetUserPhotos(socieUser.SocieId);
                        var friendProfilePicId = helper.GetProfilePictureId(socieUser);
                        friendPhotosEmotions.profilePicId = friendProfilePicId;
                        topF.Add(friendPhotosEmotions);
                    }
                }
            }

            ViewBag.topF = topF;
            string json = JsonConvert.SerializeObject(topF);
            return json;
        }
    }
}