using log4net;
using System.IO;

namespace Analyst.Facebook
{
    class PhotoResizer
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PhotoResizer));

        public void ResizePhotos(string imagesBase)
        {
            DirectoryInfo info = new DirectoryInfo(imagesBase);
            var subDirectories = info.GetDirectories();
            foreach (var dir in subDirectories)
            {
                var userId = dir.Name;
                logger.Info(string.Format("OxfordTools : Analysing user [{0}]", userId));

                var files = dir.GetFiles();
                foreach (var file in files)
                {
                    if(file.FullName.Contains("small"))
                    {
                        File.Delete(file.FullName);
                    }
                }

                files = dir.GetFiles();
                foreach(var file in files)
                {
                   Resize(file.FullName, file.FullName.Replace(".jpg", "_small.jpg"));
                }
            }
        }

        private void Resize(string imageFile, string outputFile)
        {
            using (var srcImage = System.Drawing.Image.FromFile(imageFile))
            {
                var newWidth = 250;// (int)(srcImage.Width * scaleFactor);
                var newHeight = 250; // (int)(srcImage.Height * scaleFactor);
                using (var newImage = new System.Drawing.Bitmap(newWidth, newHeight))
                {
                    using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));
                        try
                        {
                            newImage.Save(outputFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch (System.Exception)
                        {
                            return;
                        }
                        
                    }
                }
            }
        }
    }
}
