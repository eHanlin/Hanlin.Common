using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Hanlin.Common.Extensions;

namespace Hanlin.Common.Utils
{
    public class ImageUtils
    {
        public static Stream Resize(Stream imageStream, int newWidth)
        {
            var srcImage = Image.FromStream(imageStream);
            var resized = srcImage.Resize(newWidth);

            var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Png);

            return stream;
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
    }
}
