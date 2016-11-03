using Hanlin.Common.Extensions;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [TestCase(null, null)]
        [TestCase("Test", "test")]
        [TestCase("TestCase", "testCase")]
        public void ToFirstLower(string input, string expected)
        {
            Assert.AreEqual(expected, input.ToFirstLowerOrDefault());
        }

        [TestCase(null, null)]
        [TestCase("test", "Test")]
        [TestCase("testCase", "TestCase")]
        public void ToFirstUpper(string input, string expected)
        {
            Assert.AreEqual(expected, input.ToFirstUpperOrDefault());
        }
    }
}