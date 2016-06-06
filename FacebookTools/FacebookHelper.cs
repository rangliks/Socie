using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using FacebookTools.FacebookObjects;
using Project.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using log4net;

namespace FacebookTools
{
    public class FacebookHelper
    {
        private static ILog logger;

        private string imagesBase = @"C:\socie\";
        private FacebookClient client = null;
        int i = 0;
        private string accessToken = string.Empty;
        private string userId = string.Empty;
        private string name = string.Empty;
        private string email = string.Empty;

        private Person me;
        public Person Me 
        { 
            get 
            {
                return me;
            } 
        }

        public FacebookHelper(string accToken, string socieId = "")
        {
            logger = LogManager.GetLogger(typeof(FacebookHelper));

            me = new Person();
            accessToken = accToken;
            client = new FacebookClient(accToken);
            userId = getUserId();

            me.PersonId = userId;
            me.Name = name;
            me.Token = accessToken;
            
            // not all persons are socie users
            // if it's a socie user set the SocieId
            if(!string.IsNullOrEmpty(socieId))
            {
                me.SocieId = socieId;
            }
        }

        private string getUserId()
        {
            if (!string.IsNullOrEmpty(this.userId)) return this.userId;

            string uid = string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                dynamic me = client.Get("me");
                uid = me.id;
                name = me.name;

                me = client.Get("me?fields=email");
                email = me.email;
            }

            return uid;
        }

        public string getUserEmail()
        {
            dynamic me = client.Get("me?fields=email");
            return (string) me.email;
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

        public List<PhotoAlbum> GetUserAlbums()
        {
            var albums = client.Get(string.Format("/me/albums?fields=id&access_token={0}", accessToken));
            return ObjectParser.ParseAlbums(albums, me.PersonId);
        }

        public List<Photo> GetAlbumsPhotos(List<PhotoAlbum> albums)
        {
            List<Photo> photos = new List<Photo>();
            foreach (var album in albums)
            {
                /* TODO : update db with name parameter (add field to the class PhotoAlbum */
                var albumPhotos = client.Get(string.Format("/{0}/photos", album.AlbumId));
                photos.AddRange(ObjectParser.ParseAlbum(albumPhotos, album));
            }

            return photos;
        }

        public void DownloadAlbums(List<PhotoAlbum> albums)
        {
            foreach (var album in albums)
            {
                /* TODO : update db with name parameter (add field to the class PhotoAlbum */
                var albumPhotos = client.Get(string.Format("/{0}/photos", album.AlbumId));
                List<Photo> photos = ObjectParser.ParseAlbum(albumPhotos, album);
                DownloadPics(photos);
            }
        }

        public void DownloadPics(List<Photo> photos)
        {
            foreach (var photo in photos)
            {
                DownloadPicture(photo);
            }
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

        /// <summary>
        /// get profile picture for current session user
        /// </summary>
        public string GetProfilePictureId(Person person)
        {
            // api endpoint for getting profile pictures
            var url = string.Format("https://graph.facebook.com/me/picture?access_token={0}", client.AccessToken);

            // a call to get the picture download url
            var pictureUrl = GetProfilePictureUrl(person);
            return extractPicIdFromUrl(pictureUrl);
        }

        public List<Person> GetFriends(bool downloadProfilePics = true)
        {
            var friendListData = client.Get("/me/friends");
            JObject friendListJson = JObject.Parse(friendListData.ToString());

            List<Person> friends = new List<Person>();
            foreach (var friend in friendListJson["data"].Children())
            {
                Person person = new Person(friend.ToString());
                if(downloadProfilePics)
                {
                    DownloadProfilePicture(person);
                }

                person.ProfilePicId = GetProfilePictureId(person);
                friends.Add(person);
            }

            return friends;
        }

        /// <summary>
        /// get list of person of tagged users
        /// </summary>
        /// <param name="inputPhotos"></param>
        /// <returns></returns>
        public List<Person> GetMyPhotosTagsPersons(List<Photo> inputPhotos)
        {
            List<Person> persons = new List<Person>();
            foreach (var photo in inputPhotos)
            {
                var taggedInMyPhoto = client.Get(string.Format("/{0}/tags", photo.PhotoId));
                JObject taggedJson = JObject.Parse(taggedInMyPhoto.ToString());
                foreach (var tag in taggedJson["data"].Children())
                {
                    Person p = new Person(tag.ToString());

                    if (!persons.Any(x => x.PersonId == p.PersonId))
                    {
                        persons.Add(p);
                    }
                }
            }

            return persons;
        }

        public List<Tag> GetMyPhotosTags(List<Photo> inputPhotos)
        {
            List<Tag> tags = new List<Tag>();
            foreach(var photo in inputPhotos)
            {
                var taggedInMyPhoto = client.Get(string.Format("/{0}/tags", photo.PhotoId));
                JObject taggedJson = JObject.Parse(taggedInMyPhoto.ToString());
                foreach (var tag in taggedJson["data"].Children())
                {
                    Tag t = new Tag(tag.ToString(), photo.PhotoId);

                    if (!string.IsNullOrEmpty(t.TagId))
                    {
                        tags.Add(t);
                    }
                }
            }

            return tags;
        }

        //public void GetMyPhotos()
        //{
        //    var myPhotos = client.Get("/me/photos");
        //    JObject photosJson = JObject.Parse(myPhotos.ToString());
        //    Dictionary<string, Photo> photos = new Dictionary<string, Photo>();

        //    while (photosJson["paging"]["next"] != null)
        //    {
        //        foreach (var photo in photosJson["data"].Children())
        //        {
        //            Photo currentPhoto = new Photo();
        //            currentPhoto.CreationDate = DateTime.Parse(photo["created_time"].ToString());
        //            currentPhoto.PhotoId = photo["id"].ToString();

        //            var taggedInMyPhoto = client.Get(string.Format("/{0}/tags", currentPhoto.PhotoId));
        //            JObject taggedJson = JObject.Parse(taggedInMyPhoto.ToString());
        //            foreach (var tag in taggedJson["data"].Children())
        //            {
        //                Tag t = new Tag(tag.ToString());
        //                currentPhoto.Tags.Add(t);
        //                DownloadProfilePicture(t.PersonTagged);
        //            }

        //            photos.Add(currentPhoto.PhotoId, currentPhoto);
        //            DownloadPicture(currentPhoto);
        //        }

        //        myPhotos = client.Get(photosJson["paging"]["next"].ToString());
        //        photosJson = JObject.Parse(myPhotos.ToString());
        //    }


        //    //var taggedInMyPhoto = client.Get("/579827528750261/tags");

        //    var fr = client.Get("me?fields=friends");
        //}

        public List<Person> GetFamily()
        {
            var results = new List<Person>();

            //log.Debug("application.");
            var family = client.Get("me?fields=family");
            JObject familyJson = JObject.Parse(family.ToString());
            if(familyJson["family"] != null)
            {
                foreach (var relative in familyJson["family"]["data"])
                {
                    try
                    {
                        Person person = new Person(relative.ToString());
                        results.Add(person);
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            

            return results;
        }

        public void DownloadProfilePictures(List<Person> persons)
        {
            foreach (var person in persons)
            {
                DownloadProfilePicture(person);
            }
        }

        public void DownloadProfilePicture(Person person)
        {
            var picId = string.Empty;
            var url = GetProfilePictureUrl(person);
            try
            {
                picId = extractPicIdFromUrl(url);
                
                string fileName = string.Format("image_{0}.jpg", picId);
                DownloadFromUrl(url, fileName, person);
                Console.WriteLine("not fuck!");
            }
            catch (Exception)
            {
                Console.WriteLine("fuck!");
                string fileName = string.Format("profile_{0}.jpg", person.PersonId);
                DownloadFromUrl(url, fileName, person);
            }
           
        }

        private string extractPicIdFromUrl(string url)
        {
            string picId = string.Empty;

            var splitted = url.Split('/');
            try
            {
                foreach (var item in splitted)
                {
                    var innerSplit = item.Split('_');
                    if (innerSplit.Count() >= 2)
                    {
                        picId = innerSplit[1];
                    }
                }
            }
            catch (Exception)
            {
                picId = splitted[6].Split('_')[1];
            }

            return picId;
        }

        private void DownloadPicture(Photo photo)
        {
            var url = GetPhotoUrl(photo);
            if(!string.IsNullOrEmpty(url))
            {
                string filename = string.Format("image_{0}.jpg", photo.PhotoId);
                DownloadFromUrl(url, filename);
            }
        }

        public void Resize(string imageFile, string outputFile, double scaleFactor)
        {
            using (var srcImage = System.Drawing.Image.FromFile(imageFile))
            {
                var newWidth = 200;// (int)(srcImage.Width * scaleFactor);
                var newHeight = 200; // (int)(srcImage.Height * scaleFactor);
                using (var newImage = new System.Drawing.Bitmap(newWidth, newHeight))
                {
                    using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));
                        newImage.Save(outputFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
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
                var reqUrl = string.Format("https://www.facebook.com/photo/download/?fbid={0}&width=300&height=300", photo.PhotoId);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(reqUrl);
                request.UserAgent = "my user agent";
                response = request.GetResponse();
                if (response != null && request.RequestUri.Equals(response.ResponseUri))
                {
                    response.Close();
                    return null;
                }
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

        private WebResponse callUrl(string url)
        {
            WebResponse response = null;
            string pictureUrl = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                response = request.GetResponse();
                return response;
            }
            catch (Exception ex)
            {
                //? handle
            }
            finally
            {
                if (response != null) response.Close();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">the url of the image to download</param>
        /// <param name="fileName">the filename to save</param>
        /// <param name="person">using person to add to it's own directory</param>
        public void DownloadFromUrl(string url, string fileName, Person person = null)
        {
            var personid = string.Empty;
            if(person == null)
            {
                personid = Me.PersonId;
            }
            else
            {
                personid = person.PersonId;
            }

            makeSureTheresAlreadyUserDirectory(personid);
            using (WebClient webClient = new WebClient())
            {
                var path = string.Format(@"{0}\{1}\{2}", imagesBase, personid, fileName);
                logger.Info(string.Format("I want to download [{0}]", path));
                if(!File.Exists(path))
                {
                    webClient.DownloadFile(url, path);
                    logger.Info(string.Format("Downloaded pic of personid [{0}]", personid));
                }
                else
                {
                    logger.Info(string.Format("Pic of personid already exists [{0}]", personid));
                }
                
            }
        }

        private void makeSureTheresAlreadyUserDirectory(string personId = null)
        {
            string personid = string.Empty;
            if(string.IsNullOrEmpty(personId))
            {
                personid = Me.PersonId;
            }
            else
            {
                personid = personId;
            }

            string directory = string.Format(@"{0}\{1}", imagesBase, personid);
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(string.Format(@"{0}\{1}", imagesBase, personid));
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void SetDownloadPath(string imagesBase)
        {
            this.imagesBase = imagesBase;
        }
    }
}
