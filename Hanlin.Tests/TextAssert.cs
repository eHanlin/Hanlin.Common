﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hanlin.Tests
{
    public static class TextAssert
    {
        public static void BoundedBy(string text, TextBound bound)
        {
            if (bound == null) throw new ArgumentNullException("bound");

            BoundedBy(text, bound.Start, bound.End);
        }

        public static void BoundedBy(string text, string[] expectedBound)
        {
            if (expectedBound == null) throw new ArgumentNullException("expectedBound");
            if (expectedBound.Length != 2) throw new ArgumentException("expectedBound requires two elements.");

            BoundedBy(text, expectedBound[0], expectedBound[1]);
        }
        
        public static void BoundedBy(string text, string start, string end)
        {
            var trimmed = text.Trim();
            Assert.IsNotNull(trimmed);

            if (start == null && end == null) throw new ArgumentException("start and end cannot both be null");

            if (start == null)
            {
                Assert.AreEqual(trimmed, end);
            }
            else if (end == null)
            {
                Assert.AreEqual(trimmed, start);
            }
            else
            {
                Assert.IsTrue(trimmed.StartsWith(start));
                Assert.IsTrue(trimmed.EndsWith(end));
            }
        }

        public static void Contains(string text, string substring)
        {
            Assert.IsTrue(text.Contains(substring));
        }
    }
}
