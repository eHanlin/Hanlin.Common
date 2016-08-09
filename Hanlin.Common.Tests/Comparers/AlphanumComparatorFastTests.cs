using Hanlin.Common.Comparers;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Comparers
{
    class AlphanumComparatorFastTests
    {
        [TestCase("4-2", "4-10", 1)]
        [TestCase("95813-24-5-9-34", "95813-24-5-10-4", 1)]
        [TestCase("95813-24-5-10-4", "95813-24-5-10-34", 1)]
        [TestCase("１-1", "1-２", 1)]
        [TestCase("Ａ", "Ｂ", 1)]
        public void XLessThanY(string x, string y, int expected)
        {
            Assert.Less(AlphanumComparatorFast.Instance.Compare(x, y), 0);
        }
    }
}
