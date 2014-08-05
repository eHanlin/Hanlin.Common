using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common.Text
{
    public static class FullWidthCharacter
    {
        public const char Half0 = '0';
        public const char Half9 = '9';
        public const char HalfA = 'A';
        public const char HalfZ = 'Z';
        public const char Halfa = 'a';
        public const char Halfz = 'z';

        public const char Full0 = '０'; // /uFF10
        public const char Full9 = '９'; // /uFF19
        public const char FullA = 'Ａ'; // /uFF21
        public const char FullZ = 'Ｚ';
        public const char Fulla = 'ａ'; // /uFF41
        public const char Fullz = 'ｚ';

        public static string ToHalfWidth(string text)
        {
            if (text == null) return null;

            return string.Join(string.Empty, text.Select(ToHalfWidth));
        }

        public static char ToHalfWidth(char wchar)
        {
            if (wchar >= Full0 && wchar <= Full9)
            {
                return (char)(wchar - Full0 + '0');
            }

            if (wchar >= FullA && wchar <= FullZ)
            {
                return (char)(wchar - FullA + 'A');
            }

            if (wchar >= FullA && wchar <= FullZ)
            {
                return (char)(wchar - FullA + 'A');
            }

            if (wchar >= Fulla && wchar <= Fullz)
            {
                return (char)(wchar - Fulla + 'a');
            }

            return wchar;
        }
    }
}
