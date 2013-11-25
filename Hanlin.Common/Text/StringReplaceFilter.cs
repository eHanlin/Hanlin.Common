using System.Collections.Generic;

namespace Hanlin.Common.Text
{
    public class StringReplaceFilter : IStringFilter
    {
        protected Dictionary<string, string> Patterns;

        public StringReplaceFilter()
        {
            Patterns = new Dictionary<string, string>();
        }

        public string Filter(string input)
        {
            foreach (var pair in Patterns)
            {
                input = input.Replace(pair.Key, pair.Value);
            }
            return input;
        }
    }
}