using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common
{
    public static class UnicodeUtil
    {
        public const char FullWidthA = 'Ａ'; // /uFF21
        public const char FullWidthZ = 'Ｚ';
        public const char FullWidtha = 'ａ'; // /uFF41
        public const char FullWidthz = 'ｚ';
        public const char FullWidth0 = '０'; // /uFF10
        public const char FullWidth9 = '９'; // /uFF19

        public static char Normalize(char unicode)
        {
            if (unicode >= FullWidth0 && unicode <= FullWidth9)
            {
                return (char)(unicode - FullWidth0 + '0');
            }

            if (unicode >= FullWidthA && unicode <= FullWidthZ)
            {
                return (char)(unicode - FullWidthA + 'A');
            }

            if (unicode >= FullWidtha && unicode <= FullWidthz)
            {
                return (char)(unicode - FullWidtha + 'a');
            }

            return unicode;
        }
    }
}
