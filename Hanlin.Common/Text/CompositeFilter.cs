using System.Collections.Generic;

namespace Hanlin.Common.Text
{
    public class CompositeFilter : List<IStringFilter>, IStringFilter
    {
        public string Filter(string input)
        {
            return Filter(input, null);
        }

        public string Filter(string input, StringFilterOptions options)
        {
            foreach (var filter in this)
            {
                input = filter.Filter(input, options);
            }
            return input;
        }
    }
}