using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Controllers;
using Project.FacebookObjects;
using log4net;

namespace Project.Helpers
{
    public class FacebookSessionHelper : Controller
    {
        private ILog log = new TheLogger().GetLogger();
        private FacebookClient client = null;
        int i = 0;
        private string accessToken = string.Empty;
        private string userId = string.Empty;
        private string name = string.Empty;
        private string email = string.Empty;

        private Person me;

        public FacebookSessionHelper(string accToken)
        {
            me = new Person();
            accessToken = accToken;
            client = new FacebookClient(accToken);
            userId = getUserId();

            me.PersonId = userId;
            me.Name = name;
        }

        private string getUserId()
        {
            if (!string.IsNullOrEmpty(this.userId)) return this.userId;

            string uid = string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                dynamic me = client.Get("me");
                uid = me.id;
                name = me.first_name + " " + me.second_name;

                me = client.Get("me?fields=email");
                email = me.email;
            }

            return uid;
        }

        public void GetUserPosts(int numOfPostsBack)
        {
            var firstPage = client.Get(string.Format("/{0}/posts", userId));
            PostsObject pObject = JsonConvert.DeserializeObject<PostsObject>(firstPage.ToString());
            foreach (var datum in pObject.data)
            {
                var story = datum.story;
            }
        }

        public void GetUserFeed()
        {
            var firstPage = client.Get(string.Format("/{0}/feed", userId));
            PostsObject pObject = JsonConvert.DeserializeObject<PostsObject>(firstPage.ToString());
            foreach (var datum in pObject.data)
            {
                break;
            }
        }

        public void GetUserScores()
        {
            var scores = client.Get(string.Format("/{0}/scores", userId));
            //PostsObject pObject = JsonConvert.DeserializeObject<PostsObject>(firstPage.ToString());
            //foreach (var datum in pObject.data)
            //{
            //    var story = datum.story;
            //}
        }

        public void GetUserHome()
        {
            var newsFeed = client.Get(string.Format("/{0}/home", userId));
            //PostsObject pObject = JsonConvert.DeserializeObject<PostsObject>(firstPage.ToString()); 
            //foreach (var datum in pObject.data) 
            //{ 
            //    var story = datum.story;
            //}
        }

        public void GetUploadedPhotos()
        {
            var photos = client.Get(string.Format("/{0}/photos/uploaded", userId));
        }

        public void GetProfilePicture()
        {
            var url = string.Format("https://graph.facebook.com/me/picture?access_token={0}", client.AccessToken);

            var pictureUrl = GetProfilePictureUrl(me);
            DownloadFromUrl(pictureUrl);
        }

        public void GetFriends()
        {
            //dynamic friends = client.Get(string.Format("/{0}/friends", userId));
            //string nextFriend = friends.paging.next;
            //while(!string.IsNullOrEmpty(nextFriend))
            //{
            //    friends = client.Get(nextFriend);
            //    nextFriend = friends.paging.next;
            //}

            var friendListData = client.Get("/me/friends");
            JObject friendListJson = JObject.Parse(friendListData.ToString());

            List<Person> friends = new List<Person>();
            foreach (var friend in friendListJson["data"].Children())
            {
                Person person = new Person(friend.ToString());
                DownloadProfilePicture(person);

                friends.Add(person);
            }
        }

        public void GetMyPhotos()
        {
            var myPhotos = client.Get("/me/photos");
            JObject photosJson = JObject.Parse(myPhotos.ToString());
            Dictionary<string, Photo> photos = new Dictionary<string, Photo>();

            while (photosJson["paging"]["next"] != null)
            {
                foreach (var photo in photosJson["data"].Children())
                {
                    Photo currentPhoto = new Photo();
                    currentPhoto.CreationDate = DateTime.Parse(photo["created_time"].ToString());
                    currentPhoto.PhotoId = photo["id"].ToString();

                    var taggedInMyPhoto = client.Get(string.Format("/{0}/tags", currentPhoto.PhotoId));
                    JObject taggedJson = JObject.Parse(taggedInMyPhoto.ToString());
                    foreach (var tag in taggedJson["data"].Children())
                    {
                        Tag t = new Tag(tag.ToString());
                        currentPhoto.Tags.Add(t);
                        DownloadProfilePicture(t.PersonTagged);
                    }

                    photos.Add(currentPhoto.PhotoId, currentPhoto);
                    DownloadPicture(currentPhoto);
                }

                myPhotos = client.Get(photosJson["paging"]["next"].ToString());
                photosJson = JObject.Parse(myPhotos.ToString());
            }
            

            //var taggedInMyPhoto = client.Get("/579827528750261/tags");
           
            var fr = client.Get("me?fields=friends");
        }

        public void GetFamily()
        {
            log.Debug("application.");
            var family = client.Get("me?fields=family");
            JObject familyJson = JObject.Parse(family.ToString());
            foreach (var relative in familyJson["family"]["data"])
            {
                try
                {
                    Person person = new Person(relative.ToString());
                    DownloadProfilePicture(person);
                }
                catch (Exception ex)
                {
                }
                
            }

        }

        private void DownloadProfilePicture(Person person)
        {
            string fileName = string.Format("profile_{0}.jpg", person.PersonId);
            var url = GetProfilePictureUrl(person);
            DownloadFromUrl(url, fileName);
        }

        private void DownloadPicture(Photo photo)
        {
            var url = GetPhotoUrl(photo);
            string filename = string.Format("image_{0}.jpg", photo.PhotoId);
            DownloadFromUrl(url, filename);
        }

        public void DownloadPhoto(string photoId)
        {
            Photo photo = new Photo();
            photo.PhotoId = photoId;
            DownloadPicture(photo);
        }

        public static string GetPhotoUrl(Photo photo)
        {
            WebResponse response = null;
            string pictureUrl = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(string.Format("https://facebook.com/photo/download/?fbid={0}", photo.PhotoId));
                response = request.GetResponse();
                pictureUrl = response.ResponseUri.ToString();
            }
            catch (Exception ex)
            {
                //? handle
            }
            finally
            {
                if (response != null) response.Close();
            }
            return pictureUrl;
        }

        public static string GetProfilePictureUrl(Person person)
        {
            WebResponse response = null;
            string pictureUrl = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(string.Format("https://graph.facebook.com/{0}/picture?height=500", person.PersonId));
                response = request.GetResponse();
                pictureUrl = response.ResponseUri.ToString();
            }
            catch (Exception ex)
            {
                //? handle
            }
            finally
            {
                if (response != null) response.Close();
            }
            return pictureUrl;
        }

        public static void DownloadFromUrl(string url, string fileName = "iamge.jpg")
        {
            using (WebClient webClient = new WebClient())
            {
                var path = string.Format(@"C:\socie\{0}", fileName);
                webClient.DownloadFile(url, path);
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}