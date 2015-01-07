using Hanlin.Common.Text;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Text
{
    class FullWidthCharacterTests
    {
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("0", "0")]
        [TestCase("a", "a")]
        [TestCase("０", "0")]
        [TestCase("９", "9")]
        [TestCase("Ａ", "A")]
        [TestCase("Ｚ", "Z")]
        [TestCase("ａ", "a")]
        [TestCase("ｚ", "z")]
        public void ToHalfWidth(string input, string expected)
        {
            Assert.AreEqual(expected, FullWidthCharacter.ToHalfWidth(input));
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("０", "０")]
        [TestCase("ａ", "ａ")]
        [TestCase("０", "0")]
        [TestCase("９", "9")]
        [TestCase("Ａ", "A")]
        [TestCase("Ｚ", "Z")]
        [TestCase("ａ", "a")]
        [TestCase("ｚ", "z")]
        public void ToFullWidth(string expected, string input)
        {
            Assert.AreEqual(expected, FullWidthCharacter.ToFullWidth(input));
        }
    }
}
