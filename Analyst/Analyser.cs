using Analyst.Db;
using Analyst.Facebook;
using OxfordTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst
{
    public static class Analyser
    {
        public static async void Run()
        {
            //FacebookConnector connector = new FacebookConnector();
            //connector.FindPhotos();
            var v = await OxfordFaceService.FindFaces();
            DbDriver driver = new DbDriver();
            driver.SaveEmotions(v);
        }
    }
}
