using Analyst.Db;
using Analyst.Facebook;
using DbHandler.Objects;
using FacebookTools;
using FacebookTools.FacebookObjects;
using FacebookTools.SocieObjects;
using log4net;
using OxfordTools;
using OxfordTools.OxfordObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst
{
    public static class Analyser
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Analyser));

        /// <summary>
        /// The main analysis program
        /// Will run periodically and will analyse then add/update the data to db
        /// </summary>
        public static async void Run()
        {
            // unfortunatley hard coded :( get the current images directory path (for dev and production)
            var path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            var production = ConfigurationManager.AppSettings["production"].Equals("true");
            string imagesBase = string.Empty;

            if (production)
            {
                imagesBase = @"C:\socie\Content\images";
            }
            else
            {
                try
                {
                    imagesBase = string.Format("{0}\\Project\\Content\\images", Directory.GetParent(path).FullName);
                }
                catch (Exception e)
                {
                }
            }
            

            // create a facebook connector which will handle the data access from facebook
            // the data will be saved to db and photos will be downloaded to the drive
            FacebookConnector connector = new FacebookConnector();
            connector.FindPhotos(imagesBase);

            // resize photos for UI needs
            PhotoResizer resizer = new PhotoResizer();
            resizer.ResizePhotos(imagesBase);

            // Find the photos not already analysed and send them to be analysed
            DbDriver driver = new DbDriver();
            var photosAlreadyAnalyzed = driver.GetEmotions();
            var socieUsers = driver.GetSocieUsers();

            // async operation - go analyse the photos
            List<EmotionScores> newEmotions = await OxfordFaceService.FindFaces(photosAlreadyAnalyzed, socieUsers, false, imagesBase);


            // Create the notifications - find high avg emotions and add new notifications to db
            // This actually - send a notification to a user from user
            List<PersonPhotoWithAlbum> emotionsOwners = GetUsersOfNewEmotions(newEmotions);
            var dictNewEmotions = newEmotions.GroupBy(x => x.PhotoId).ToDictionary(x => x.Key, y => y.ToList());
            foreach (var item in dictNewEmotions)
            {
                var avgs = new List<double>() { item.Value.Average(x => x.happiness), item.Value.Average(x => x.sadness), item.Value.Average(x => x.anger) };
                if(avgs.Any(x => x > 0.9))
                {
                    Logger.Info("Analyzer adding new notification");
                    AddNotification(emotionsOwners, avgs, item.Key);
                }
            }
            
            // save the new emotions to db
            driver.SaveEmotions(newEmotions);
            Logger.Info(string.Format("Completed saving emotions to db. [{0}] emotions added", newEmotions.Count));
        }

        /// <summary>
        /// creates a notification to send to all friends of a person about highly emotional photo
        /// </summary>
        /// <param name="emotionsOwners">the person/album/photo which is the owner of the image</param>
        /// <param name="avgs">the scores of this photo</param>
        /// <param name="photoId">the highly emotional photo</param>
        private static void AddNotification(List<PersonPhotoWithAlbum> emotionsOwners, List<double> avgs, string photoId)
        {
            DbDriver driver = new DbDriver();

            var currentPhoto = emotionsOwners.Single(x => x.photo.PhotoId.Equals(photoId));
            FacebookHelper helper = new FacebookHelper(currentPhoto.person.Token);
            var friends = helper.GetFriends(false);

            Notification selfNot = new Notification();
            selfNot.FromPerson = currentPhoto.person.PersonId;
            selfNot.ToPerson = currentPhoto.person.PersonId;
            selfNot.PhotoId = photoId;
            selfNot.Message = "self " + getMessage(avgs);
            driver.SaveNotification(selfNot);

            foreach(var friend in friends)
            {
                Notification notify = new Notification();
                notify.FromPerson = currentPhoto.person.PersonId;
                notify.ToPerson = friend.PersonId;
                notify.PhotoId = photoId;
                notify.Message = getMessage(avgs);

                driver.SaveNotification(notify);
            }
        }

        private static string getMessage(List<double> avgs)
        {
            //happiness
            if(avgs[0] > 0.9)
            {
                return "happiness";
            }
            //sadness
            if (avgs[0] > 0.9)
            {
                return "surprise";
            }
            //anger
            if (avgs[0] > 0.9)
            {
                return "anger";
            }

            return "neutral";
        }

        private static List<EmotionScores> FilterEmotions(List<EmotionScores> newEmotions)
        {
            List<EmotionScores> scoresFiltered = new List<EmotionScores>();
            double filterThreshold = 0.9;
            foreach (var emotion in newEmotions)
            {
                if (emotion.happiness > filterThreshold || emotion.sadness > filterThreshold || emotion.surprise > filterThreshold || emotion.neutral > filterThreshold)
                {
                    scoresFiltered.Add(emotion);
                }
            }

            return scoresFiltered;
        }

        public static List<PersonPhotoWithAlbum> GetUsersOfNewEmotions(List<EmotionScores> scores)
        {
            List<string> photoIds = scores.Select(x => x.PhotoId).ToList();
            DbDriver driver = new DbDriver();
            List<PersonPhotoWithAlbum> users = driver.GetPersonsOfPhotos(photoIds);

            return users;
        }
    }
}
