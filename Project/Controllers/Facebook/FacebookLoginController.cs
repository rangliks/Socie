using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;

namespace Project.Controllers.Facebook
{
    public class FacebookLoginController : Controller
    {
        //
        // GET: /FacebookLogin/
        public ActionResult Index()
        {
            
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                /* <add key="Facebook:AppId" value="201303350205233" />
    <add key="Facebook:AppSecret" value="94c3effc0f986cb482944496d0c53480" />*/
                client_id = ConfigurationManager.AppSettings.Get("Facebook:AppId"),
                client_secret = ConfigurationManager.AppSettings.Get("Facebook:AppSecret"),
                grant_type = "client_credentials"
            });
            Session["AccessToken"] = result.access_token;

            fb.AccessToken = result.access_token;
            //var client = new FacebookClient(result.access_token);
            dynamic me = fb.Get("me/friends");
            string firstName = me.first_name;
            string lastName = me.last_name;
            string email = me.email;

            return View();
        }
	}
}