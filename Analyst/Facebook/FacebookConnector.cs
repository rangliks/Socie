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
            foreach(var person in persons)
            {
                FacebookHelper myHelper = new FacebookHelper(person.Token);
                driver.SavePerson(myHelper.Me, person.SocieId);

                List<PhotoAlbum> albums = myHelper.GetUserAlbums();
                myHelper.SaveAlbumsToDB(albums);
                driver.SaveAlbums(albums);
                myHelper.DownloadAlbums(albums);
                var albumPhotos = myHelper.GetAlbumsPhotos(albums);
                driver.SavePhotos(albumPhotos);
                myHelper.GetProfilePicture();
            }

            //var persons = driver.getPersons();
            //foreach(var person in persons)
            //{
            //    FacebookHelper helper = new FacebookHelper(person.Token);
            //    //helper.GetUserHome();
            //    //helper.GetUserFeed();
            //    //helper.GetUserScores();
            //    //helper.GetUploadedPhotos();
            //    //helper.GetFriends();
                
            //    helper.GetUserAlbums();
            //    helper.GetProfilePicture();
            //    //helper.DownloadPhoto("10154134457642682");
            //}
        }
    }
}
