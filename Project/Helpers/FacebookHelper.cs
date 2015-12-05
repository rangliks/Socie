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

namespace Project.Helpers
{
    public class FacebookSessionHelper : Controller
    {
        private FacebookClient client = null;
        private string accessToken = string.Empty;
        private string userId = string.Empty;
        private string name = string.Empty;
        private string email = string.Empty;


        public FacebookSessionHelper(string accToken)
        {
            accessToken = accToken;
            client = new FacebookClient(accToken);
            userId = getUserId();
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
                var story = datum.story;
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
            var picture = client.Get(string.Format("/{0}/music", userId));
        }
    }
}