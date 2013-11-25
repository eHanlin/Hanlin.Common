using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Hanlin.Common.Text
{
    public class PatternRemoveFilter : IStringFilter
    {
        protected StringCollection Patterns = new StringCollection();

        public string Filter(string input)
        {
            foreach (string pattern in Patterns)
            {
                input = Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);
            }

            return input;
        }
    }

    public class EmptyLineFilter : PatternRemoveFilter
    {
        public EmptyLineFilter()
        {
            Patterns.Add("\r\n\r\n");
        }
    }
}