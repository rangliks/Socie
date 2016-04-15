using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Emotion;
using System.IO;
using System.Net;

namespace OxfordTools
{
    class Program
    {
        static void Main(string[] args)
        {
            getClient();
            System.Console.ReadLine();
        }

        static async void getClient()
        {
            //EmotionServiceClient emotionClient = new EmotionServiceClient("4c37362995694b17b93e259ba9087bbf");
            //using (Stream s = File.OpenRead(@"C:\Users\Ran\Downloads\ran2.jpg"))
            //{
            //    var emotions = await emotionClient.RecognizeAsync(s);
            //    foreach (var emo in emotions)
            //    {
            //        var v = emo.Scores;
            //    }
            //}

            FaceServiceClient client = new FaceServiceClient("8a0b69482d234dffa7d425acc5b06ecc");
            DirectoryInfo info = new DirectoryInfo(@"C:\socie");
            var files = Directory.GetFiles(@"c:\socie");
            foreach (var file in files)
            {
                using (Stream s = File.OpenRead(file))
                {
                    var faces = await client.DetectAsync(s, true, true);
                    //var faces = await client.
                    foreach (var face in faces)
                    {
                        var rect = face.FaceRectangle;
                        var landmarks = face.FaceLandmarks;

                        double noseX = landmarks.NoseTip.X;
                        double noseY = landmarks.NoseTip.Y;

                        var attributes = face.FaceAttributes;

                        Console.Write(face.ToString());
                    }
                }
            }

            using (Stream s = File.OpenRead(@"C:\Users\Ran\Downloads\ran2.jpg"))
            {
                var faces = await client.DetectAsync(s, true, true);
                foreach (var face in faces)
                {
                    var rect = face.FaceRectangle;
                    var landmarks = face.FaceLandmarks;

                    double noseX = landmarks.NoseTip.X;
                    double noseY = landmarks.NoseTip.Y;

                    var attributes = face.FaceAttributes;

                    Console.Write(face.ToString());
                }
            }
        }
    }
}
