﻿using System.Collections.Generic;
using System.Linq;
using FacebookTools;
using FacebookTools.FacebookObjects;
using log4net;
using System.IO;

namespace Analyst.Facebook
{
    public class FacebookConnector
    {
        DbHandler.Db.DbDriver driver;
        private static ILog logger;// = LogManager.GetLogger(typeof(FacebookConnector));
        

        public FacebookConnector()
        {
            logger = LogManager.GetLogger(typeof(FacebookConnector));
            driver = new DbHandler.Db.DbDriver();
        }

        public void FindPhotos(string imagesBase)
        {
            var persons = driver.getPersons();
            foreach (var person in persons)
            {
                logger.Info(string.Format("Checking inPerson name [{0}]", person.Name));
                if (!string.IsNullOrEmpty(person.Token))
                {
                    // create api session with this user
                    logger.Info(string.Format("Found socie user [{0}] id [{1}]", person.Name, person.PersonId));
                    logger.Info(string.Format("Creating Facebook Helper for [{0}]", person.Name));
                    FacebookHelper myHelper = new FacebookHelper(person.Token);

                    // set the download path we got in the input params
                    myHelper.SetDownloadPath(imagesBase);

                    // update user credentials
                    logger.Info(string.Format("Saving inPerson name [{0}]", person.Name));
                    //driver.SavePerson(myHelper.Me, inPerson.SocieId);

                    // get albums and save to db if selfNot already exists
                    logger.Info(string.Format("Finding inPerson albums. name [{0}]", person.Name));
                    List<PhotoAlbum> albums = myHelper.GetUserAlbums();
                    logger.Info(string.Format("Saving inPerson albums. name [{0}]", person.Name));
                    driver.SaveAlbums(albums);

                    // download all pics from each inAlbum
                    logger.Info(string.Format("Downloading inPerson albums. name [{0}]", person.Name));
                    myHelper.DownloadAlbums(albums);

                    // get from api a list of all pics in the albums
                    logger.Info(string.Format("Getting Photos information. name [{0}]", person.Name));
                    var albumPhotos = myHelper.GetAlbumsPhotos(albums);

                    // get all pics tagged people and add them to db
                    logger.Info(string.Format("Getting tagged people. name [{0}]", person.Name));
                    var taggedPeersons = myHelper.GetMyPhotosTagsPersons(albumPhotos);
                    logger.Info(string.Format("Got {0} tagged persons for [{1}]'s images", taggedPeersons.Count(), person.Name));

                    logger.Info("Saving tagged people to db");
                    driver.SavePersons(taggedPeersons);

                    logger.Info("Downloading tagged people profile pics");
                    myHelper.DownloadProfilePictures(taggedPeersons);

                    logger.Info(string.Format("Getting tags of all inPhoto albums. name [{0}]", person.Name));
                    var tags = myHelper.GetMyPhotosTags(albumPhotos);

                    // save the pics to db if selfNot already exists
                    logger.Info(string.Format("Saving all photos to db. name [{0}]", person.Name));
                    driver.SavePhotos(albumPhotos);

                    logger.Info(string.Format("Saving tags to db. name [{0}]", person.Name));
                    driver.SaveTags(tags);

                    // download current user profile pic
                    logger.Info(string.Format("Downloading {0}'s profile pic", person.Name));
                    myHelper.DownloadProfilePicture(person);

                    // download user family profile pics
                    logger.Info(string.Format("Getting {0}'s family", person.Name));
                    var family = myHelper.GetFamily();

                    //foreach (var famMember in family)
                    //{
                    //    myHelper.GetMemberPhotos(famMember);
                    //}

                    logger.Info(string.Format("Downloading {0}'s family profile pics", person.Name));
                    myHelper.DownloadProfilePictures(family);

                    logger.Info(string.Format("Done getting pics for [{0}]", person.Name));
                }
            }

            logger.Info("Done finding photos");
        }


        /// <summary>
        /// Import external data with csv file
        /// pics should be added manually
        /// </summary>
        public void ImportData()
        {
            // read csv of the pics to import
            string[] lines = System.IO.File.ReadAllLines(@"C:\socie\pics.csv");

            // build objects for albums and photos
            List<PhotoAlbum> albums = new List<PhotoAlbum>();
            List<Photo> photosByAlbumId = new List<Photo>();
            bool first = true;
            foreach (var line in lines)
            {
                if (first)
                {
                    first = false;
                    continue;
                }

                PhotoCsvRecord record = new PhotoCsvRecord(line);

                PhotoAlbum album = new PhotoAlbum();
                album.PersonId = record.PersonId;
                album.AlbumId = record.AlbumId;
                album.Name = record.Name;

                if (!albums.Any(x => x.AlbumId == record.AlbumId))
                {
                    albums.Add(album);
                }

                if (!photosByAlbumId.Any(x => x.PhotoId == record.PhotoId) && !string.IsNullOrEmpty(album.PersonId))
                {
                    Photo photo = new Photo();
                    photo.PhotoId = record.PhotoId;
                    photo.Name = record.Name;
                    photo.AlbumId = album.AlbumId;

                    photosByAlbumId.Add(photo);
                }
            }

            driver.SaveAlbums(albums);
            driver.SavePhotos(photosByAlbumId);
        }
    }
}
