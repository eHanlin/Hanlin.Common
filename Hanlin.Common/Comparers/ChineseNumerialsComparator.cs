using System.Collections;
using System.Collections.Generic;
using Hanlin.Common.Text;

namespace Hanlin.Common.Comparers
{
    public class ChineseNumerialsComparator : IComparer<string>, IComparer
    {
        public static readonly ChineseNumerialsComparator Instance = new ChineseNumerialsComparator();

        public int Compare(string x, string y)
        {
            var xInArabicNumerals = ChineseNumerals.ConvertToArabicNumerals(x);
            var yInArabicNumerals = ChineseNumerals.ConvertToArabicNumerals(y);

            return AlphanumComparatorFast.Instance.Compare(xInArabicNumerals, yInArabicNumerals);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as string, y as string);
        }
    }
}
