using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.IO;
using Microsoft.ProjectOxford.Emotion;
using System.Reflection;
using System.Threading;
using OxfordTools.OxfordObjects;
using log4net;
using FacebookTools.FacebookObjects;
//using MyLogger;

namespace OxfordTools
{
    public static class OxfordFaceService
    {
        //private DbDriver db;
        //public OxfordFaceService()
        //{
        //    db = new DbDriver();
        //}
        private static ILog Logger = LogManager.GetLogger(typeof(OxfordFaceService));

        public static async Task<List<EmotionScores>> FindFaces(List<EmotionScores> oldScores, List<Person> socieUsers, bool viewPicturesInPaint = false, string imagesBase = @"C:\socie")
        {
            Logger.Info("OxfordTools : Starting find faces and emotions");
            // object to return
            List<EmotionScores> scores = new List<EmotionScores>();

            // open connection to microsoft services
            EmotionServiceClient emotionClient = new EmotionServiceClient("4c3736xxxxxxxxxxxxxxxxxx87bbf");
            FaceServiceClient client = new FaceServiceClient("4c3736xxxxxxxxxxxxxxxxxx87bbf");

            // foreach of the pics directories pass over and recognize feelings
            DirectoryInfo info = new DirectoryInfo(imagesBase);
            var subDirectories = info.GetDirectories();
            foreach (var dir in subDirectories)
            {
                Logger.Info(string.Format("OxfordTools : Checking directory [{0}]", dir.Name));
                var userId = dir.Name;
                if(socieUsers.Any(x=> x.PersonId.Equals(userId)))
                {
                    var username = socieUsers.Where(x => x.PersonId.Equals(userId)).First().Name;
                    Logger.Info(string.Format("OxfordTools : Found socie user, Analysing [{0}]", username));

                    var files = dir.GetFiles();
                    foreach (var file in files)
                    {
                        string photoIdFromFile = string.Empty;
                        try
                        {
                            photoIdFromFile = file.Name.Split('_')[1].Split('.')[0];
                            if (oldScores.Any(x => x.PhotoId.Equals(photoIdFromFile)))
                            {
                                Logger.Info(string.Format("skipping already analyzed image. photoif [{0}]", photoIdFromFile));
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                        
                        }

                        if (file.Name.Contains("small")) continue;
                        try
                        {
                            using (Stream s = file.OpenRead())
                            {
                                var isImage = file.Name.Split('_')[0] == "image";
                                if (isImage)
                                {
                                    var emotions = await emotionClient.RecognizeAsync(s);

                                    if (emotions.Any())
                                    {
                                        var photoId = file.Name.Split('_')[1].Split('.')[0];
                                        Logger.Info(string.Format("OxfordTools : Found {0} emotions for photoId [{1}] user [{2}] ", emotions.Count(), photoId, userId));
                                        System.Diagnostics.Process paint = null;

                                        if (viewPicturesInPaint)
                                        {
                                            System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                                            procInfo.FileName = ("mspaint.exe");
                                            procInfo.Arguments = file.FullName;
                                            paint = System.Diagnostics.Process.Start(procInfo);
                                        }

                                        foreach (var emo in emotions)
                                        {
                                            var v = emo.Scores;
                                            EmotionScores scoresObj = new EmotionScores(emo, photoId);
                                            var msg = string.Format("-------------\n happy [{0}]\n surprise [{1}]\n sad[{2}]\n neutral [{3}]\n anger [{4}]\n disguast [{5}]\n", v.Happiness, v.Surprise, v.Sadness, v.Neutral, v.Anger, v.Disgust);
                                            Logger.Info(msg);
                                            System.Threading.Thread.Sleep(500);
                                            scores.Add(scoresObj);
                                        }

                                        if (paint != null)
                                        {
                                            paint.Kill();
                                            Thread.Sleep(2000);
                                        }
                                    }
                                    else
                                    {
                                        Logger.Info(string.Format("No Emotions found. file [{0}]", file.Name));
                                    }
                                }
                                else
                                {
                                    Logger.Info(string.Format("Not valid image!!!!! [{0}]", file.Name));
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                
            }

            return scores;
        }
    }
}
