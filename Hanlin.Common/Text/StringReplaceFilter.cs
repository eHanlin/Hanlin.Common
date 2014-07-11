using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Hanlin.Common.Text
{
    public class StringReplaceFilter : IStringFilter
    {
        private readonly IDictionary<string, string> Patterns = new Dictionary<string, string>();

        public string Filter(string input)
        {
            foreach (var pattern in Patterns)
            {
                input = Regex.Replace(input, pattern.Key, pattern.Value, RegexOptions.IgnoreCase);
            }

            return input;
        }

        public void Replace(string regex, string replaceRegex)
        {
            Patterns[regex] = replaceRegex;
        }

        public void Remove(string regex)
        {
            Patterns[regex] = string.Empty;
        }
    }

    public class EmptyLineFilter : StringReplaceFilter
    {
        public EmptyLineFilter()
        {
            Remove("\r\n\r\n");
        }
    }
}