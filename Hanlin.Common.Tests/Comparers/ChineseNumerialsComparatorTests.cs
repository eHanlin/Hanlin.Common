using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanlin.Common.Comparers;
using Hanlin.Common.Text;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Comparers
{
    class ChineseNumerialsComparatorTests
    {
        [TestCase("第二冊", "第三冊")]
        [TestCase("第四冊", "第五冊")]
        public void LessThanCompare(string x, string y)
        {
            Assert.Less(ChineseNumerialsComparator.Instance.Compare(x, y), 0);
        }

        [Test]
        public void NumeralOrdering()
        {
            var unicodeBasedOrder = ChineseNumerals.NormalChineseNumerals.OrderBy(n => n);
            var numeralNaturalOrder = ChineseNumerals.NormalChineseNumerals.Select(n => n.ToString())
                .OrderBy(n => n, ChineseNumerialsComparator.Instance).Select(s => s.First()).ToList();

            Console.WriteLine(string.Join(", ", unicodeBasedOrder));

            Console.WriteLine(string.Join(", ", numeralNaturalOrder));

            CollectionAssert.AreNotEqual(unicodeBasedOrder, ChineseNumerals.NormalChineseNumerals);

            CollectionAssert.AreEqual(numeralNaturalOrder, ChineseNumerals.NormalChineseNumerals);
        }
    }
}
