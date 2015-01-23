using Hanlin.Common.Text;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Text
{
    class ChineseNumeralsTests
    {
        [TestCase("零", "0")]
        [TestCase("一", "1")]
        [TestCase("二", "2")]
        [TestCase("三", "3")]
        [TestCase("四", "4")]
        [TestCase("五", "5")]
        [TestCase("六", "6")]
        [TestCase("七", "7")]
        [TestCase("八", "8")]
        [TestCase("九", "9")]
        [TestCase("十", "10")]
        [TestCase("十一", "11")]
        [TestCase("十二", "12")]
        [TestCase("十三", "13")]
        [TestCase("一十一", "11")]
        [TestCase("一十", "10")]
        [TestCase("二十", "20")]
        [TestCase("十五", "15")]
        [TestCase("二十三", "23")]
        [TestCase("第十三章", "第13章")]
        [TestCase("第十冊 第十一章", "第10冊 第11章")]
        [TestCase("第二十冊 第九十一章", "第20冊 第91章")]
        public void Convert(string input, string expected)
        {
            Assert.AreEqual(expected, ChineseNumerals.ConvertToArabicNumerals(input));
        }

        [TestCase("零", "零")]
        [TestCase("十", "一零")]
        [TestCase("一十", "一零")]
        [TestCase("二十", "二零")]
        [TestCase("十五", "一五")]
        [TestCase("二十三", "二三")]
        [TestCase("第十三章", "第一三章")]
        public void ConvertBases(string input, string expected)
        {
            Assert.AreEqual(expected, ChineseNumerals.Literalize(input));
        }

        [TestCase("第十冊 第十一章", "第一零冊 第一一章")]
        [TestCase("第二十冊 第九十一章", "第二零冊 第九一章")]
        [TestCase("十一，二十，十，四十五", "一一，二零，一零，四五")]
        public void ConvertBasesRecursed(string input, string expected)
        {
            Assert.AreEqual(expected, ChineseNumerals.Literalize(input));
        }


        [TestCase('零', true)]
        [TestCase('一', true)]
        [TestCase('二', true)]
        [TestCase('三', true)]
        [TestCase('四', true)]
        [TestCase('五', true)]
        [TestCase('六', true)]
        [TestCase('七', true)]
        [TestCase('八', true)]
        [TestCase('九', true)]
        [TestCase('十', false)]
        [TestCase('1', false)]
        public void IsChineseDigit(char ch, bool expected)
        {
            Assert.AreEqual(expected, ChineseNumerals.IsChineseNumeral(ch));
        }
    }
}
