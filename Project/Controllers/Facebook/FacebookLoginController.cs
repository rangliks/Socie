using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using System.Net;
using System.IO;
using Project.Helpers;

namespace Project.Controllers.Facebook
{
    public class FacebookLoginController : Controller
    {
        public ViewResult Index()
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
                    FacebookSessionHelper helper = new FacebookSessionHelper(token);
                    helper.GetUserFeed();
                    helper.GetUserScores();
                    helper.GetUploadedPhotos();
                    helper.GetProfilePicture();
                    helper.GetFamily();
                }
            }

            return View("Index");
        }
        //
        // GET: /FacebookLogin/
    //    public ActionResult Index()
    //    {
            
    //        var fb = new FacebookClient();
    //        dynamic result = fb.Get("oauth/access_token", new
    //        {
    //            /* <add key="Facebook:AppId" value="201303350205233" />
    //<add key="Facebook:AppSecret" value="94c3effc0f986cb482944496d0c53480" />*/
    //            client_id = ConfigurationManager.AppSettings.Get("Facebook:AppId"),
    //            client_secret = ConfigurationManager.AppSettings.Get("Facebook:AppSecret"),
    //            grant_type = "client_credentials"
    //        });
    //        Session["AccessToken"] = result.access_token;

    //        fb.AccessToken = result.access_token;
    //        //var client = new FacebookClient(result.access_token);
    //        dynamic me = fb.Get("me/friends");
    //        string firstName = me.first_name;
    //        string lastName = me.last_name;
    //        string email = me.email;

    //        return View();
    //    }
	}
}