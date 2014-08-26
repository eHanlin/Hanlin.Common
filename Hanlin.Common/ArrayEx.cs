using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common
{
    public static class ArrayEx
    {
        public static bool IsOrdered(int[] array)
        {
            for (int i = 0; i < array.Count() - 1; i++)
            {
                if (array[i] > array[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if the given sorted array is sequential without skipping or duplicating elements.
        /// </summary>
        /// <param name="array">The given array must be sorted.</param>
        /// <returns></returns>
        public static bool IsSequential(int[] array)
        {
            Array.Sort(array);

            // Check for non-consecutive number. This would also catch duplicate elements.
            for (int i = 0; i < array.Count() - 1; i++)
            {
                if (array[i] != array[i + 1] - 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
