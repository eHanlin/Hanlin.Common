using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common.Text
{
    /// <summary>
    /// Currently supports only numbers in the range of 0 to 99.
    /// </summary>
    public class ChineseNumerals
    {
        public static readonly IReadOnlyCollection<char> NormalChineseNumerals = new[]
        {
            '零',
            '一',
            '二',
            '三',
            '四',
            '五',
            '六',
            '七',
            '八',
            '九'
        };

        public static string ConvertToArabicNumerals(string text)
        {
            var literalized = Literalize(text);

            var sb = new StringBuilder(literalized);

            for (int i = 0; i < sb.Length; i++)
            {
                var ch = sb[i];
                if (IsChineseNumeral(ch))
                {
                    sb[i] = ConvertToArabicNumerals(ch);
                }
            }

            return sb.ToString();
        }

        internal static string Literalize(string text)
        {
            var tenIndex = text.IndexOf('十');
            bool hasDigitBefore, hasDigitAfter;

            if (tenIndex == -1)
            {
                return text;
            }

            if (tenIndex == 0)
            {
                hasDigitBefore = false;
            }
            else
            {
                var charBefore10 = text[tenIndex - 1];
                hasDigitBefore = IsChineseNumeral(charBefore10);
            }

            if (tenIndex == text.Length - 1)
            {
                hasDigitAfter = false;
            }
            else
            {
                var charAfter10 = text[tenIndex + 1];
                hasDigitAfter = IsChineseNumeral(charAfter10);
            }

            var tmp = new StringBuilder(text);

            // 十
            if (!hasDigitBefore && !hasDigitAfter)
            {
                tmp.Remove(tenIndex, 1);
                tmp.Insert(tenIndex, "一零");
            }

            // 十一 到 十九 => 一一，一二，... ，一九
            else if (!hasDigitBefore && hasDigitAfter)
            {
                tmp[tenIndex] = '一';
            }

            // 一十，二十，三十等 => 一零，二零，三零
            else if (hasDigitBefore && !hasDigitAfter)
            {
                tmp[tenIndex] = '零';
            }

            // 一十二 => 一二
            else if (hasDigitBefore && hasDigitAfter)
            {
                tmp.Remove(tenIndex, 1);
            }

            text = tmp.ToString();

            if (text.Contains('十'))
            {
                return Literalize(text);
            }
            else
            {
                return text;
            }
        }

        public static bool IsChineseNumeral(char ch)
        {
            return NormalChineseNumerals.Contains(ch);
        }

        public static char ConvertToArabicNumerals(char chineseDigit)
        {
            switch (chineseDigit)
            {
                case '零': return '0';
                case '一': return '1';
                case '二': return '2';
                case '三': return '3';
                case '四': return '4';
                case '五': return '5';
                case '六': return '6';
                case '七': return '7';
                case '八': return '8';
                case '九': return '9';
                default:
                    return chineseDigit;
            }
        }
    }
}
