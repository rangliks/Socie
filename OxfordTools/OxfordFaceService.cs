using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.IO;

namespace OxfordTools
{
    public static class OxfordFaceService
    {
        public static async void FindFaces()
        {
            FaceServiceClient client = new FaceServiceClient("8a0b69482d234dffa7d425acc5b06ecc");
            string imageUrl = "http://news.microsoft.com/ceo/assets/photos/06_web.jpg";
            var faces = await client.DetectAsync(imageUrl, true, true);
 
            foreach (var face in faces)
            {
                var rect = face.FaceRectangle;
                var landmarks = face.FaceLandmarks;
            }

            DirectoryInfo info = new DirectoryInfo(@"C:\socie");
            var files = Directory.GetFiles(@"c:\socie");
            foreach (var file in files)
            {
                using (Stream s = File.OpenRead(file))
                {
                    //var faces = await client.DetectAsync(s, true, true, true, true);
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
            
        }
    }
}
