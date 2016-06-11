using Analyst.Db;
using Analyst.Facebook;
using OxfordTools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst
{
    public static class Analyser
    {
        public static async void Run()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            var production = ConfigurationManager.AppSettings["production"].Equals("true");
            string imagesBase = string.Format("{0}\\Project\\Content\\images", Directory.GetParent(path).FullName);
            if(production)
            {
                imagesBase = @"C:\socie\Content\images";
            }

            FacebookConnector connector = new FacebookConnector();
            connector.FindPhotos(imagesBase);

            PhotoResizer resizer = new PhotoResizer();
            resizer.ResizePhotos(imagesBase);

            //connector.ImportData();

            DbDriver driver = new DbDriver();
            var photosAlreadyAnalyzed = driver.GetEmotions();

            var v = await OxfordFaceService.FindFaces(photosAlreadyAnalyzed, false, imagesBase);
            
            driver.SaveEmotions(v);
        }
    }
}
