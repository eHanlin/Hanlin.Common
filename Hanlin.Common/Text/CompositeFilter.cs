using System.Collections.Generic;

namespace Hanlin.Common.Text
{
    public class CompositeFilter : List<IStringFilter>, IStringFilter
    {
        public string Filter(string input)
        {
            return Filter<StringFilterOptions>(input, null);
        }

        public string Filter<T>(string input, T options) where T : StringFilterOptions
        {
            foreach (var filter in this)
            {
                input = filter.Filter(input, options);
            }
            return input;
        }
    }
}