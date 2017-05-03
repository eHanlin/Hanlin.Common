using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hanlin.Common.Text
{
    public class GenericStringReplaceFilter<T> : IStringFilter<T> where T : StringFilterOptions
    {
        private readonly IDictionary<string, string> Patterns = new Dictionary<string, string>();

        public string Filter(string input)
        {
            return Filter(input, null);
        }

        public string Filter(string input, T options)
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

    public class StringReplaceFilter : GenericStringReplaceFilter<StringFilterOptions>
    {
        
    }

    public class EmptyLineFilter : StringReplaceFilter
    {
        public EmptyLineFilter()
        {
            Remove("\r\n\r\n");
        }
    }

    public class HtmlReplaceFilter: StringReplaceFilter
    {
        public HtmlReplaceFilter()
        {
            Remove(@"<[^>]+>|&nbsp;");
        }
    }
}