using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanlin.Common.Text;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Text
{
    class FullWidthCharacterTests
    {
        [TestCase(null, null)]
        [TestCase("", "")]
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
    }
}
