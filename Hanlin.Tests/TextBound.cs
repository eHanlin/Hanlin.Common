using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Tests
{
    public class TextBound
    {
        public const char BoundSeparator = ',';

        public string Start { get; set; }
        public string End { get; set; }

        public TextBound(string bounds)
        {
            var split = bounds.Split(BoundSeparator);
            
            Start = split[0];

            var count = split.Count();
            
            if (count > 2)
            {
                throw new ArgumentException(string.Format("Invalid bounds format: {0}. Expect: {1}", bounds, "<start>" + BoundSeparator + "<end>"));
            }

            if (count > 1)
            {
                End = split[1];
            }
        }
    }
}
