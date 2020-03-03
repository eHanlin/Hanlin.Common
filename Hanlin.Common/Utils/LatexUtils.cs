using System.Text.RegularExpressions;

namespace Hanlin.Common.Utils
{
    public class LatexUtils
    {
        public static bool IsValid(string text)
        {
            var r = new Regex(@"(\\[a-zA-Z]+)|(_\{)|(\^{)");
            return r.Match(text).Success;
        }
    }
}