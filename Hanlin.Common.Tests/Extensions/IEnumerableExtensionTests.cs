using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Hanlin.Common.Extensions;

namespace Hanlin.Common.Tests.Extensions
{
    class IEnumerableExtensionTests
    {
        [Test]
        public void TakeRandom()
        {
            var input = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var randomList = input.TakeRandom(10);

            Console.WriteLine(string.Join(", ", randomList));
        }
    }
}
