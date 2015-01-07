using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Hanlin.Tests;
using NUnit.Framework;
using Hanlin.Common.Extensions;

namespace Hanlin.Common.Tests.Extensions
{
    public class ImageExtensionTests : TestsBase
    {
        public ImageExtensionTests()
        {
            UsePath("Images");
        }

        [TestCase("LessThanMinHeight.png")]
        [TestCase("GreaterThanMinHeight.png")]
        [TestCase("ScaleToLessThanMinHeight.png")]
        public void PadToMinHeight(string file)
        {
            var expectedFilename = Path.GetFileNameWithoutExtension(file) + "_output.png";
            
            using (var original = Image.FromFile(PathTo(file)))
            using (var resized = original.Resize(original.Width, 25))
            {
                resized.Save(expectedFilename, ImageFormat.Png);
            }

            Assert.True(File.Exists(expectedFilename));
        }

        [TestCase("ScaleToLessThanMinHeight.png", 44)]
        public void Scale_MaintainMinHeight(string file, int width)
        {
            var expectedFilename = Path.GetFileNameWithoutExtension(file) + "_output.png";

            using (var original = Image.FromFile(PathTo(file)))
            using (var resized = original.Resize(width, 42))
            {
                resized.Save(expectedFilename, ImageFormat.Png);
            }

            Assert.True(File.Exists(expectedFilename));
        }
    }
}
