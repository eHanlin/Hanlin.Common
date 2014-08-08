using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Hanlin.Common.Extensions;

namespace Hanlin.Common.Utils
{
    public class ImageUtils
    {
        /// <summary>
        /// Resize in-place.
        /// </summary>
        public static void ResizeInPlace(MemoryStream imageStream, int newWidth)
        {
            if (imageStream.Length == 0) throw new ArgumentException("Input stream length cannot be zero");
            
            var srcImage = Image.FromStream(imageStream);
            var resized = srcImage.Resize(newWidth);

            imageStream.Position = 0;

            resized.Save(imageStream, ImageFormat.Png);

            // Reset stream position so that the stream can be more easily used again by the caller.
            imageStream.Position = 0;
        }

        public static Stream Resize(Stream imageStream, int newWidth)
        {
            if (imageStream.Length == 0) throw new ArgumentException("Input stream length cannot be zero");

            var srcImage = Image.FromStream(imageStream);
            var resized = srcImage.Resize(newWidth);

            var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Png);

            return stream;
        }

        public static void CropEmptySpaceInPlace(MemoryStream imageMemoryStream)
        {
            var bmp = new Bitmap(imageMemoryStream);
            using (var cropped = CropEmptySpace(bmp))
            {
                imageMemoryStream.Position = 0;
                cropped.Save(imageMemoryStream, ImageFormat.Png);
            }
        }

        public static Bitmap CropEmptySpace(Stream image)
        {
            var bmp = new Bitmap(image);
            return CropEmptySpace(bmp);
        }

        /// <summary>
        /// Code straight from: http://stackoverflow.com/a/10392379/494297
        /// 
        /// Empty space is defined as completely white or transparent.
        /// The algorithm only checks for the R components of pixels.
        /// 
        /// </summary>
        public static Bitmap CropEmptySpace(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                {
                    var pixel = bmp.GetPixel(i, row);
                    if (pixel.R != 255 && pixel.A != 0)
                        return false;
                }
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                {
                    var pixel = bmp.GetPixel(col, i);
                    if (pixel.R != 255 && pixel.A != 0)
                        return false;
                }

                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);
            }
        }

        public static Stream Combine(IReadOnlyCollection<MemoryStream> imageStreams)
        {
            if (imageStreams == null) throw new ArgumentNullException("imageStreams");
            if (!imageStreams.Any()) throw new ArgumentException("imageStream cannot be empty.");

            var images = imageStreams.Select(Image.FromStream);
            var outputStream = new MemoryStream();

            using (var combinedImage = images.Aggregate((image1, image2) =>
            {
                try
                {
                    return image1.Combine(image2, 0);
                }
                finally
                {
                    image1.Dispose();
                    image2.Dispose();
                }
            }))
            {
                combinedImage.Save(outputStream, ImageFormat.Png);
            }

            return outputStream;
        }
    }
}
