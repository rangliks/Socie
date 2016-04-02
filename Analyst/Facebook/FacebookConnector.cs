using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookTools;

namespace Analyst.Facebook
{
    class FacebookConnector
    {
        private FacebookHelper helper;
        public FacebookConnector(string token)
        {
            helper = new FacebookHelper(token);
        }


    }
}
