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

namespace OxfordTools
{
    public static class OxfordFaceService
    {
        //private DbDriver db;
        //public OxfordFaceService()
        //{
        //    db = new DbDriver();
        //}

        public static async Task<List<EmotionScores>> FindFaces(bool viewPicturesInPaint = false)
        {
            List<EmotionScores> scores = new List<EmotionScores>();

            EmotionServiceClient emotionClient = new EmotionServiceClient("4c37362995694b17b93e259ba9087bbf");
            FaceServiceClient client = new FaceServiceClient("8a0b69482d234dffa7d425acc5b06ecc");

            //string imageUrl = "http://news.microsoft.com/ceo/assets/photos/06_web.jpg";
            //var faces = await client.DetectAsync(imageUrl, true, true);
 
            //foreach (var face in faces)
            //{
            //    var rect = face.FaceRectangle;
            //    var landmarks = face.FaceLandmarks;
            //}

            DirectoryInfo info = new DirectoryInfo(@"C:\socie");
            var subDirectories = info.GetDirectories();
            foreach (var dir in subDirectories)
            {
                var userId = dir.Name;
                var files = dir.GetFiles();
                foreach (var file in files)
                {
                    //try
                    //{
                    //    using (Stream s = file.OpenRead())
                    //    {
                    //        var faces = await client.DetectAsync(s, true, true);
                    //        foreach (var face in faces)
                    //        {
                    //            var rect = face.FaceRectangle;
                    //            var landmarks = face.FaceLandmarks;

                    //            double noseX = landmarks.NoseTip.X;
                    //            double noseY = landmarks.NoseTip.Y;

                    //            var attributes = face.FaceAttributes;

                    //            Console.Write(face.ToString());
                    //        }
                    //    }
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}
                    
                    try
                    {
                        using (Stream s = file.OpenRead())
                        {
                            var emotions = await emotionClient.RecognizeAsync(s);
                            if (emotions.Any())
                            {
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
                                    EmotionScores scoresObj = new EmotionScores(emo, file.Name.Split('_')[1].Split('.')[0]);
                                    var msg = string.Format("-------------\n happy [{0}]\n surprise [{1}]\n sad[{2}]\n neutral [{3}]\n anger [{4}]\n disguast [{5}]\n", v.Happiness, v.Surprise, v.Sadness, v.Neutral, v.Anger, v.Disgust);
                                    Console.Write(msg);
                                    System.Threading.Thread.Sleep(3000);
                                    scores.Add(scoresObj);
                                }

                                if(paint != null)
                                {
                                    paint.Kill();
                                    Thread.Sleep(2000);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return scores;
        }
    }
}
