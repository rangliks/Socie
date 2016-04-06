using Analyst.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using FacebookTools.FacebookObjects;
using Analyst.Facebook;
using OxfordTools;
namespace Analyst
{
    class Program
    {
        static void Main(string[] args)
        {
            //FacebookConnector connector = new FacebookConnector();
            //connector.FindPhotos();
            OxfordFaceService service = new OxfordFaceService();
            OxfordFaceService.FindFaces();
        }
    }
}
