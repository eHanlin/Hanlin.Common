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
    }
}
