using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Hanlin.Common.Extensions
{
    public static class ImageExtensions
    {
        public static Image Resize(this Image srcImage, int newWidth)
        {
            var ratio = (double) newWidth / srcImage.Size.Width;

            var newHeight = (int) (srcImage.Height * ratio);

            // Resizing code from: http://stackoverflow.com/a/87786/494297
            var newImage = new Bitmap(newWidth, newHeight);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
            }

            return newImage;
        }

        // Reference: http://stackoverflow.com/a/465199
        public static Stream Combine(this Image topImg, Image bottomImg)
        {
            const int margin = 20; // in pixels

            var newSize = new Size
            {
                Width = topImg.Width,
                Height = topImg.Height + bottomImg.Height + margin
            };

            var returnStream = new MemoryStream();
            using (var bitmap = new Bitmap(newSize.Width, newSize.Height))
            {
                using (var canvas = Graphics.FromImage(bitmap))
                {
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    canvas.DrawImage(topImg, 0, 0);
                    canvas.DrawImage(bottomImg, 0, topImg.Height + margin);
                    canvas.Save();
                }
                bitmap.Save(returnStream, ImageFormat.Png);
            }
            return returnStream;
        }
    }
}
