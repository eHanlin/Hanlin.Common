using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Hanlin.Common.Extensions;

namespace Hanlin.Common
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
    }
}
