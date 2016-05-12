using Analyst.Db;
using Analyst.Facebook;
using OxfordTools;
using System;
using System.Collections.Generic;
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
            string imagesBase = string.Format("{0}\\Project\\Content\\images", Directory.GetParent(path).FullName);

            FacebookConnector connector = new FacebookConnector();
            connector.FindPhotos(imagesBase);

            PhotoResizer resizer = new PhotoResizer();
            resizer.ResizePhotos(imagesBase);

            //connector.ImportData();

            var v = await OxfordFaceService.FindFaces();
            DbDriver driver = new DbDriver();
            driver.SaveEmotions(v);
        }
    }
}
