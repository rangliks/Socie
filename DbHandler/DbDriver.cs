using FacebookTools.FacebookObjects;
using OxfordTools.OxfordObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbHandler.Extensions;
using DbHandler.Objects;
using log4net;
using FacebookTools.SocieObjects;

namespace DbHandler.Db
{
    public class DbDriver
    {
        private readonly string connectionString;
        private SocieContext db;
        private static ILog logger;

        public DbDriver()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["SocieContext"].ConnectionString;
            }
            catch (Exception ex)
            {
                connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }

            db = new SocieContext(connectionString);
            logger = LogManager.GetLogger(typeof(DbDriver));
        }

        public void SaveEmotion(EmotionScores emo)
        {
            db.EmotionScores.Add(emo);
            db.SaveChanges();
        }

        //public Photo GetPhoto(string photoId)
        //{
        //    var photos = from inPhoto
        //                 in db.Photo
        //                 where inPhoto.PhotoId == photoId
        //                 select new { }
        //}

        public string getSocieUserRealName(string socieId)
        {
            var user = from usr
                in db.Person
                where usr.SocieId == socieId
                select new {Name = usr.Name};

            try
            {
                if (user.Any())
                {
                    return user.First().Name;
                }
            }
            catch (Exception ex)
            {
            }
            

            return string.Empty;

        }

        /// <summary>
        /// Get All Persons Data from db (Person table)
        /// </summary>
        /// <returns>List of all persons in db</returns>
        public List<Person> getPersons()
        {
            var users = from person
                in db.Person
                select person;

            return users.ToList();
            //List<Person> persons = new List<Person>();
            //foreach (var user in users)
            //{
            //    Person inPerson = new Person();
            //    inPerson.Name = user.Name;
            //    inPerson.Token = user.Token;
            //    inPerson.PersonId = user.PersonId;
            //    inPerson.SocieId = user.PersonId;

            //    persons.Add(inPerson);
            //}

            //return persons;
        }

        public void SaveAlbums(List<PhotoAlbum> albums)
        {
            var currentAlbums = from photoAlbum
                in db.PhotoAlbum
                select new {AlbumId = photoAlbum.AlbumId, Name = photoAlbum.Name, PersonId = photoAlbum.PersonId};
            var currentAlbumsDictionary = currentAlbums.ToDictionary(x => x.AlbumId);
            foreach (var album in albums)
            {
                if (currentAlbumsDictionary.ContainsKey(album.AlbumId))
                {
                    /*/ UPDATE EXISTING /*/
                }
                else
                {
                    db.PhotoAlbum.Add(album);
                }
            }

            db.SaveChanges();
        }

        public void SavePhotos(List<Photo> albumPhotos)
        {
            var currentPhotos = from photo
                in db.Photo
                select new {PhotoId = photo.PhotoId, Name = photo.Name, CreationDate = photo.CreationDate};
            var currentPhotosDictionary = currentPhotos.ToDictionary(x => x.PhotoId);
            foreach (var photo in albumPhotos)
            {
                if (currentPhotosDictionary.ContainsKey(photo.PhotoId))
                {
                    /*/ UPDATE EXISTING /*/
                    Photo photoTemp = db.Photo.Where(x => x.PhotoId == photo.PhotoId).FirstOrDefault();
                    if(photo != null)
                    {
                        photoTemp.AlbumId = photo.AlbumId;
                    }
                }
                else
                {
                    db.Photo.Add(photo);
                }
            }

            db.SaveChanges();
        }

        public Person SavePerson(Person person, string socieId)
        {
            var personInDB = from me
                in db.Person
                where me.PersonId == person.PersonId
                select me;

            // if not exist in db save it
            // else update record
            if (!personInDB.Any())
            {
                db.Person.Add(person);
            }
            else
            {
                foreach (Person p in personInDB)
                {
                    if (!string.IsNullOrEmpty(person.Token))
                    {
                        p.Token = person.Token;
                    }
                    
                    p.SocieId = socieId;
                    break;
                }
            }

            db.SaveChanges();
            return person;
        }

        public UserPhotosEmotions GetUserPhotos(string socieId)
        {
            UserPhotosEmotions userPhotosEmotions = new UserPhotosEmotions();

            //Dictionary<Photo, double> bestHappiest = new Dictionary<Photo, double>();

            // get current user from Person table
            var socieUser = from usr in db.Person
                where usr.SocieId == socieId
                select usr;

            Person user = socieUser.FirstOrDefault();
            userPhotosEmotions.userId = user.PersonId;
            userPhotosEmotions.userName = user.Name;

            // get the current user inPhoto albums
            //var albums = from photoAlbums in db.PhotoAlbum
            //             where photoAlbums.PersonId == user.PersonId
            //             select photoAlbums;

            //List<string> albumIds = albums.Select(x => x.AlbumId).ToList();

            // with the albumids get the photos
            var photos = from photo in db.Photo
                join album in db.PhotoAlbum on photo.AlbumId equals album.AlbumId
                where album.PersonId == user.PersonId
                select photo;

            // put only ids of all photos into list
            //List<string> photoIds = photos.Select(x => x.PhotoId).ToList();
            //List<Photo> allPhotos = photos.ToList();

            // get all emotions connected to the inPhoto ids
            var photosEmotions = from emotion in db.EmotionScores
                join photo in photos on emotion.PhotoId equals photo.PhotoId
                select emotion;

            // map emotions to a < photoid => list of emotions > dictionary
            Dictionary<string, List<EmotionScores>> emotionsDictionary = photosEmotions.GroupBy(pe => pe.PhotoId).ToDictionary(pe => pe.Key,
                pe => pe.ToList());
            //    new Dictionary<string, List<EmotionScores>>();
            //foreach(EmotionScores emotion in photosEmotions)
            //{
            //    if(!emotionsDictionary.ContainsKey(emotion.PhotoId))
            //    {
            //        emotionsDictionary.Add(emotion.PhotoId, new List<EmotionScores>());
            //    }

            //    emotionsDictionary[emotion.PhotoId].Add(emotion);
            //}

            List<PhotoAndEmotions> dynamicPhotos = new List<PhotoAndEmotions>();
            foreach (var emotions in emotionsDictionary)
            {
                List<EmotionScores> generalEmotions = emotions.Value;
                if (generalEmotions != null)
                {
                    EmotionScores avg = new EmotionScores();
                    Photo photo = photos.FirstOrDefault(x => x.PhotoId == emotions.Key);
                    avg.happiness = emotions.Value.Average(x => x.happiness);
                    avg.anger = emotions.Value.Average(x => x.anger);
                    avg.contempt = emotions.Value.Average(x => x.contempt);
                    avg.disgust = emotions.Value.Average(x => x.disgust);
                    avg.fear = emotions.Value.Average(x => x.fear);
                    avg.sadness = emotions.Value.Average(x => x.sadness);
                    avg.surprise = emotions.Value.Average(x => x.surprise);
                    dynamicPhotos.Add(new PhotoAndEmotions { photo = photo, emotions = avg });
                }
            }

            userPhotosEmotions.photosEmotions = dynamicPhotos;

            var totalImages = userPhotosEmotions.photosEmotions.Count();
            if (totalImages < 5)
            {
                var photosList = photos.ToList().Take(6);
                foreach (var item in photosList)
                {
                    if(!userPhotosEmotions.photosEmotions.Any(x => x.photo.PhotoId == item.PhotoId))
                    {
                        PhotoAndEmotions photoAndEmo = new PhotoAndEmotions();
                        photoAndEmo.emotions = new EmotionScores();
                        photoAndEmo.photo = item;

                        userPhotosEmotions.photosEmotions.Add(photoAndEmo);
                    }
                }
                
            }

            return userPhotosEmotions;
            //return dynamicPhotos;
            //List<PhotoAndEmotions> finalEmotions = dynamicPhotos.OrderByDescending(r => r.emotions.happiness).Take(5).ToList();
            //return finalEmotions;
        }

        public string getPersonId(string p)
        {
            var person = from user
                in db.Person
                where user.SocieId == p
                select user;

            return person.FirstOrDefault().PersonId;
        }

        public Person GetPerson(string socieid)
        {
            var person = from user
                in db.Person
                where user.SocieId == socieid
                select user;

            return person.FirstOrDefault();
        }

        public Person getSocieUser(string personId)
        {
            var result = from user
                in db.Person
                where user.PersonId == personId
                select user;

            return result.FirstOrDefault();
        }

        public void SavePersons(List<Person> taggedPeersons)
        {
            var allPersons = from person
                in db.Person
                select person;

            var personsIds = allPersons.Select(x => x.PersonId).Distinct().ToList();
            var taggedDistinct = taggedPeersons.Distinct().ToList();
            foreach (var tagged in taggedDistinct)
            {
                if (!string.IsNullOrEmpty(tagged.PersonId) && !personsIds.Contains(tagged.PersonId))
                {
                    db.Person.Add(tagged);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {

                throw;
            }


        }

        public List<EmotionScores> GetPhotoEmotions(string photoId)
        {
            var myEmotions = from emotions
                           in db.EmotionScores
                           where emotions.PhotoId == photoId
                           select emotions;
            return myEmotions.ToList();
        }
        public void SaveTags(List<Tag> tags)
        {
            var allPhotos = from photo
                            in db.Photo
                            select photo;
            var allphotosList = allPhotos.ToList();

            var allTagsInDb = from existingTag
                    in db.Tag
                    select existingTag;
            var keys = allTagsInDb.Select(x => x.TagId).ToList();

            HashSet<string> insertedKeys = new HashSet<string>();
            foreach (var tag in tags)
            {
                //var p = from existingTag
                //    in db.Tag
                //    where existingTag.TagId == tag.TagId
                //    select existingTag;
                bool photoExist = allphotosList.Any(x => x.PhotoId == tag.PhotoId);
                bool tagAlreadyInserted = insertedKeys.Contains(tag.TagId);
                bool tagAlreadyInDb = keys.Contains(tag.TagId);
                if (photoExist && !tagAlreadyInserted && !tagAlreadyInDb)
                {
                    db.Tag.Add(tag);
                    insertedKeys.Add(tag.TagId);
                }
                else
                {
                    logger.Info(string.Format("DbDriver tag will not be inserted. photoExist [{0}] tagAlreadyInserted [{1}] tagAlreadyInDb [{2}]", photoExist, tagAlreadyInserted, tagAlreadyInDb));
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {

                throw;
            }
        }

        public List<PhotoAndEmotions> GetUserPhotosByEmotion(string socieId, Emotion emotion, int takeCount)
        {

            IOrderedEnumerable<PhotoAndEmotions> orderedPhotos = null;
            switch (emotion)
            {
                    case Emotion.Anger:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.anger);
                    break;
                    case Emotion.Contempt:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.contempt);
                    break;
                    case Emotion.Disgust:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.disgust);
                    break;
                    case Emotion.Fear:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.fear);
                    break;
                    case Emotion.Happiness:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.happiness);
                    break;
                    case Emotion.Sadness:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.sadness);
                    break;
                    case Emotion.Surprise:
                    orderedPhotos = GetUserPhotos(socieId).photosEmotions.OrderByDescending(p => p.emotions.surprise);
                    break;
            }
            return orderedPhotos.Take(takeCount).ToList();
        }

        public List<Notification> GetUserNotifications(string personId, int totalRecords = 5)
        {
            var notifications = (from notification
                                in db.Notifications
                                 where notification.ToPerson == personId
                                 orderby notification.CreationDate descending
                                 select notification).Take(totalRecords);

            return notifications.ToList();
                                
        }

        /// <summary>
        /// get the inPerson objects of the owners of input photoIds
        /// </summary>
        /// <param name="photoIds"></param>
        /// <returns></returns>
        public PersonPhotoWithAlbum GetPersonsOfPhotos(string photoId)
        {
            var myUser = (from user
                        in db.Person
                        join photoAlbum in db.PhotoAlbum on user.PersonId equals photoAlbum.PersonId
                        join photo in db.Photo on photoAlbum.AlbumId equals photo.AlbumId
                        where photoId == photo.PhotoId
                        select new { User = user, Album = photoAlbum, Photo = photo }).FirstOrDefault();


            return myUser != null ? new PersonPhotoWithAlbum(myUser.User, myUser.Album, myUser.Photo) : null;
        }
    }

    public enum Emotion
    {
        Happiness,
        Anger,
        Contempt ,
        Disgust,
        Fear,
        Sadness,
        Surprise
    }
}