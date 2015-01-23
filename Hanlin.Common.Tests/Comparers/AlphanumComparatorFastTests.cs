using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanlin.Common.Comparers;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Comparers
{
    class AlphanumComparatorFastTests
    {
        [TestCase("4-2", "4-10", 1)]
        public void XLessThanY(string x, string y, int expected)
        {
            Assert.Less(AlphanumComparatorFast.Instance.Compare(x, y), 0);
        }
    }
}
