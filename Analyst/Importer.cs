using Analyst.Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst
{
    /// <summary>
    /// this class is used for debug needs, it can import raw data to insert into db
    /// used to insert data that is not real from facebook like external photo albums of a user
    /// </summary>
    public class Importer
    {
        string DownloadsDirectory = @"C:\Users\Ran\Downloads";
        public void ImportData()
        {
            FacebookConnector connector = new FacebookConnector();
            connector.ImportData();
        }

        public void SetFilenames()
        {
            var files = Directory.GetFiles(DownloadsDirectory);
            foreach (var file in files)
            {
                var picId = file.Split('_')[1];
                var newName = string.Format("{0}\\image_{1}.jpg", DownloadsDirectory, picId);
                File.Move(file, newName);
            }
            
        }
    }
}
