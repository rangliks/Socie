using DbHandler.Db;
using DbHandler.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Users
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            string photoId = Request.Params["photoid"];
            DbDriver driver = new DbDriver();
            ViewBag.emotions = driver.GetPhotoEmotions(photoId);
            PersonPhotoWithAlbum personPhoto = driver.GetPersonsOfPhotos(photoId);
            ViewBag.userid = personPhoto.person.PersonId;
            ViewBag.photoid = personPhoto.photo.PhotoId;
            return View();
        }
    }
}