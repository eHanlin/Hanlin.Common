using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Hanlin.Common
{
    public static class ProcessHelper
    {
        public static Process[] GetWordProcesses()
        {
            return Process.GetProcessesByName("WINWORD");
        }

        public static bool WaitForWordToExit(int waitTimeMillis = 30000)
        {
            const int waitSlice = 50;

            if (waitTimeMillis < waitSlice) return false;

            int tries = waitTimeMillis / waitSlice;
            do
            {
                Thread.Sleep(50);
                tries -= 1;
            } while (GetWordProcesses().Any() && tries > 0);

            return tries > 0;
        }

        public static void KillWord()
        {
            Kill("WINWORD");
        }

        public static void Kill(string procName)
        {
            var procs = Process.GetProcessesByName(procName);
            foreach (var tmp in procs)
            {
                tmp.Kill();
            }
        }
    }
}
