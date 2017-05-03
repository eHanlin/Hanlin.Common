using System.Collections.Generic;

namespace Hanlin.Common.Text
{
    public class CompositeFilter<T> : List<IStringFilter<T>>, IStringFilter<T> where T : StringFilterOptions
    {
        public string Filter(string input)
        {
            return Filter(input, null);
        }

        public string Filter(string input, T options)
        {
            foreach (var filter in this)
            {
                input = filter.Filter(input, options);
            }
            return input;
        }
    }
}