using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace Project.Helpers
{
    public class TheLogger
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ILog GetLogger()
        {
            return log;
        }
    }
}