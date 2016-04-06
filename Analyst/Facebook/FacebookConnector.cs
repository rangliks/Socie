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
        DbDriver driver;
        public FacebookConnector()
        {
            driver = new DbDriver();
        }

        public void FindPhotos()
        {
            FacebookHelper myHelper = new FacebookHelper("CAAC3FZAauWzEBALPZC2Tj2VGQRs5ZAHIq0D4N0ZCDZAaQ0ALlJoV44lyYF0CULENKwq4ioiYKWmgV1Bn91MH7o6adQY9TEzW7ZAoBRkku2aZBuZA7ZAcZCKZBL0oGXdcjp7iyVB2qlgy6xgjAgU0mdLQMnO9ZAIYYh7t1JuDTj8Tx77ZBPNylSZBsFMuAybp4dMxtp9DDeOxn62NSRgAZDZD");
            driver.SavePerson(myHelper.Me);

            List<PhotoAlbum> albums = myHelper.GetUserAlbums();
            //myHelper.SaveAlbumsToDB(albums);
            driver.SaveAlbums(albums);
            //myHelper.DownloadAlbums(albums);
            var albumPhotos = myHelper.GetAlbumsPhotos(albums);
            driver.SavePhotos(albumPhotos);

            var persons = driver.getPersons();
            foreach(var person in persons)
            {
                FacebookHelper helper = new FacebookHelper(person.Token);
                //helper.GetUserHome();
                //helper.GetUserFeed();
                //helper.GetUserScores();
                //helper.GetUploadedPhotos();
                //helper.GetFriends();
                
                helper.GetUserAlbums();
                helper.GetProfilePicture();
                //helper.DownloadPhoto("10154134457642682");
            }
        }
    }
}
