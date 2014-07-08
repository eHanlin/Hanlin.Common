using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanlin.Common.Utils;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Utils
{
    internal class ContentMemoryStreamTests
    {
        [Test]
        public void CopyEmbedSize()
        {
            var expected = new ContentMemoryStream("Test", ContentType.png) { EmbedSize = new Size(10, 22) };
            var actual = expected.Copy();

            Assert.AreEqual(expected.ContentType, actual.ContentType);
            Assert.AreEqual(expected.EmbedSize, actual.EmbedSize);
        }
        
}
}
