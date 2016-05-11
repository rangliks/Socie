using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookTools;
using Analyst.Db;
using FacebookTools.FacebookObjects;
using MyLogger;

namespace Analyst.Facebook
{
    public class FacebookConnector
    {
        DbHandler.Db.DbDriver driver;
        public FacebookConnector()
        {
            driver = new DbHandler.Db.DbDriver();
        }

        public void FindPhotos()
        {
            var persons = driver.getPersons();
            foreach (var person in persons)
            {
                Logger.Info(string.Format("Checking person name [{0}]", person.Name));
                if (!string.IsNullOrEmpty(person.Token))
                {
                    // create api session with this user
                    Logger.Info(string.Format("Found socie user [{0}] id [{1}]", person.Name, person.PersonId));
                    Logger.Info(string.Format("Creating Facebook Helper for [{0}]", person.Name));
                    FacebookHelper myHelper = new FacebookHelper(person.Token);

                    // update user credentials
                    Logger.Info(string.Format("Saving person name [{0}]", person.Name));
                    //driver.SavePerson(myHelper.Me, person.SocieId);

                    // get albums and save to db if not already exists
                    Logger.Info(string.Format("Finding person albums. name [{0}]", person.Name));
                    List<PhotoAlbum> albums = myHelper.GetUserAlbums();
                    Logger.Info(string.Format("Saving person albums. name [{0}]", person.Name));
                    driver.SaveAlbums(albums);

                    // download all pics from each album
                    Logger.Info(string.Format("Downloading person albums. name [{0}]", person.Name));
                    myHelper.DownloadAlbums(albums);

                    // get from api a list of all pics in the albums
                    Logger.Info(string.Format("Getting Photos information. name [{0}]", person.Name));
                    var albumPhotos = myHelper.GetAlbumsPhotos(albums);

                    // get all pics tagged people and add them to db
                    Logger.Info(string.Format("Getting tagged people. name [{0}]", person.Name));
                    var taggedPeersons = myHelper.GetMyPhotosTagsPersons(albumPhotos);
                    Logger.Info(string.Format("Got {0} tagged persons for [{1}]'s images", taggedPeersons.Count(), person.Name));

                    Logger.Info("Saving tagged people to db");
                    driver.SavePersons(taggedPeersons);

                    Logger.Info("Downloading tagged people profile pics");
                    myHelper.DownloadProfilePictures(taggedPeersons);

                    Logger.Info(string.Format("Getting tags of all photo albums. name [{0}]", person.Name));
                    var tags = myHelper.GetMyPhotosTags(albumPhotos);

                    Logger.Info(string.Format("Saving tags to db. name [{0}]", person.Name));
                    driver.SaveTags(tags);

                    // save the pics to db if not already exists
                    Logger.Info(string.Format("Saving all photos to db. name [{0}]", person.Name));
                    driver.SavePhotos(albumPhotos);

                    // download current user profile pic
                    Logger.Info(string.Format("Downloading {0}'s profile pic", person.Name));
                    myHelper.DownloadProfilePicture(person);

                    // download user family profile pics
                    Logger.Info(string.Format("Getting {0}'s family", person.Name));
                    var family = myHelper.GetFamily();

                    Logger.Info(string.Format("Downloading {0}'s family profile pics", person.Name));
                    myHelper.DownloadProfilePictures(family);

                    Logger.Info(string.Format("Done getting pics for [{0}]", person.Name));
                }
            }

            Logger.Info("Done finding photos");
        }

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
                    photo.Album = album;

                    photosByAlbumId.Add(photo);
                }
            }

            driver.SaveAlbums(albums);
            driver.SavePhotos(photosByAlbumId);
        }
    }
}
