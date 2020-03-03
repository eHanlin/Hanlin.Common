using Hanlin.Common.Utils;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Utils
{
    public class LatexUtilsTests
    {
        [TestCase(@"log_{x}", true)]
        [TestCase(@"\overline{x}", true)]
        [TestCase(@"\frac {x}{y}", true)]
        [TestCase(@"X_{y}", true)]
        [TestCase(@"X^{y}", true)]
        public void Property(string text, bool result)
        {
            Assert.IsTrue(LatexUtils.IsValid(text) == result);
        }
    }
}