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
        public static Image Resize(this Image srcImage, int newWidth, int minHeight = 0)
        {
            var ratio = (double) newWidth / srcImage.Size.Width;

            var newHeight = (int) (srcImage.Height*ratio);

            var canvasWidth = newWidth; // This method only support resize by width at the moment.
            var canvasHeight = Math.Max(newHeight, minHeight);

            // Resizing code from: http://stackoverflow.com/a/87786/494297
            var newImage = new Bitmap(canvasWidth, canvasHeight);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;

                // Draw the image with vertically middle alignment when the minimum height is larger.
                var drawAtY = canvasHeight > newHeight ? (canvasHeight - newHeight) / 2 : 0;

                gr.DrawImage(srcImage, new Rectangle(0, drawAtY, newWidth, newHeight));
            }

            return newImage;
        }

        // Reference: http://stackoverflow.com/a/465199
        public static Image Combine(this Image topImg, Image bottomImg, int spacingPixels = 10)
        {
            var newSize = new Size
            {
                Width = topImg.Width,
                Height = topImg.Height + bottomImg.Height + spacingPixels
            };

            var bitmap = new Bitmap(newSize.Width, newSize.Height);
            using (var canvas = Graphics.FromImage(bitmap))
            {
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                canvas.DrawImage(topImg, 0, 0);
                canvas.DrawImage(bottomImg, 0, topImg.Height + spacingPixels);
                canvas.Save();
            }
            return bitmap;
        }
    }
}
