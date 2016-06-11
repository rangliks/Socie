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
using System.Threading;
namespace Analyst
{
    class Program
    {
        static void Main(string[] args)
        {
            //Importer importer = new Importer();
            //importer.SetFilenames();

            Analyser.Run();
            System.Console.ReadLine();
        }
    }
}
