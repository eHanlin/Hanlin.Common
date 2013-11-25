using System.Collections.Generic;

namespace Hanlin.Common.Text
{
    public class CompositeFilter : List<IStringFilter>, IStringFilter
    {
        public string Filter(string input)
        {
            foreach (var filter in this)
            {
                input = filter.Filter(input);
            }
            return input;
        }
    }
}