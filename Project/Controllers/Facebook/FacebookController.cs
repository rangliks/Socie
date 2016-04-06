using Project.Helpers;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using FacebookTools.FacebookObjects;
using DbHandler.Db;
using FacebookTools;

namespace Project.Controllers.Facebook
{
    public class FacebookController : Controller
    {
        private string facebookToken;
        // GET: Facebook
        // Here We Should always redirect to home!!!!
        public ActionResult Index()
        {
            FacebookLogin();
            return RedirectToAction("Index", "Home");
        }

        public void FacebookLogin()
        {
            var appId = ConfigurationManager.AppSettings.Get("facebook_app_id");
            var appSecret = ConfigurationManager.AppSettings.Get("facebook_app_secret");
            var permissions = new PermissionsScope();
            var scope = permissions.AllPermissionsGetParams();
            var absUri = Request.Url.AbsoluteUri;
            var code = Request["code"];
            var token = string.Empty;
            if (code == null)
            {
                string authUrl =
                    string.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}",
                        appId,
                        scope,
                        absUri
                        );
                Response.Redirect(authUrl);
            }
            else
            {
                string accessTokenUri =
                    string.Format(
                        "https://graph.facebook.com/oauth/access_token?client_id={0}&scope={1}&redirect_uri={2}&code={3}&client_secret={4}",
                        appId,
                        scope,
                        absUri,
                        code.ToString(),
                        appSecret
                        );

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessTokenUri);
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    var vals = reader.ReadToEnd();

                    Dictionary<string, string> responseParams = new Dictionary<string, string>();
                    var parameters = vals.Split('&');
                    foreach (var parameter in parameters)
                    {
                        var spl = parameter.Split('=');
                        responseParams.Add(spl[0], spl[1]);
                    }

                    token = responseParams.ContainsKey("access_token") ? responseParams["access_token"] : string.Empty;
                    facebookToken = token;
                    bool exists = isUserExist(token);
                    if(!exists)
                    {
                        CreatePerson(token);
                    }
                    RedirectToAction("Index", "Home");
                }
            }

            //return RedirectToAction("Index", "User");
        }

        private bool isUserExist(string token)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var id = User.Identity.GetUserId();
            User.Identity.GetUserName();
            var users = from person 
                        in db.Person
                        where person.SocieId == id
                        select new
                        {
                            SocieId = person.SocieId,
                            PersonId = person.PersonId
                        };

            return users.Count() > 0;
            
        }

        /// <summary>
        /// Create Person entity for the user in current session
        /// </summary>
        /// <param name="token">create with current token</param>
        /// <returns></returns>
        private bool CreatePerson(string token)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            FacebookHelper helper = new FacebookHelper(token, User.Identity.GetUserId());
            DbDriver driver = new DbDriver();
            driver.SavePerson(helper.Me);

            /* TODO: add validation */
            return true;
        }
    }
}