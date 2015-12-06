using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Helpers
{
    public class PermissionsScope
    {
        HashSet<string> permissions = new HashSet<string>()
        {
            "user_birthday",
            "user_friends",
            "user_status",
            "user_posts",
            "user_videos",
            "manage_pages",
            "public_profile",
            "email",
            "user_games_activity",
            "user_photos",
            "user_actions.music",
            "user_relationships"
            //"read_stream",
            //"friend_photos",
            //"friend_status"
        };

        public string AllPermissionsGetParams()
        {
            return String.Join(",", permissions);
        }
    }
}