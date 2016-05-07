using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookTools;
using Analyst.Db;
using FacebookTools.FacebookObjects;

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
                if (!string.IsNullOrEmpty(person.Token))
                {
                    // create api session with this user
                    FacebookHelper myHelper = new FacebookHelper(person.Token);

                    // update user credentials
                    //driver.SavePerson(myHelper.Me, person.SocieId);

                    // get albums and save to db if not already exists
                    List<PhotoAlbum> albums = myHelper.GetUserAlbums();
                    driver.SaveAlbums(albums);

                    // download all pics from each album
                    //myHelper.DownloadAlbums(albums);

                    // get from api a list of all pics in the albums
                    var albumPhotos = myHelper.GetAlbumsPhotos(albums);

                    // get all pics tagged people and add them to db
                    var taggedPeersons = myHelper.GetMyPhotosTagsPersons(albumPhotos);
                    driver.SavePersons(taggedPeersons);
                    myHelper.DownloadProfilePictures(taggedPeersons);

                    var tags = myHelper.GetMyPhotosTags(albumPhotos);
                    driver.SaveTags(tags);

                    // save the pics to db if not already exists
                    driver.SavePhotos(albumPhotos);

                    // download current user profile pic
                    myHelper.DownloadProfilePicture(person);

                    // download user family profile pics
                    var family = myHelper.GetFamily();
                    myHelper.DownloadProfilePictures(family);
                }
            }
        }
    }
}
