using System.Collections.Generic;
using System.Diagnostics;

namespace Hanlin.Common
{
    public class CmdHelper
    {
        public static Process Shutdown(bool force = true, int second = 10)
        {
            var opts = new List<string> { "-s" };

            if (force) opts.Add("-f");

            opts.Add("-t");
            opts.Add("10");

            return Process.Start("shutdown", string.Join(" ", opts));
        }
    }
}