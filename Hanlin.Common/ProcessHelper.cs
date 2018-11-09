using System;
using System.ComponentModel;
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
            return WaitForProcessesToExit(() => GetWordProcesses().Any(), waitTimeMillis);
        }

        private static bool WaitForProcessesToExit(Func<bool> hasAnyProcess, int waitTimeMillis = 30000)
        {
            const int waitSlice = 50;

            if (waitTimeMillis < waitSlice) return false;

            int tries = waitTimeMillis / waitSlice;
            do
            {
                Thread.Sleep(50);
                tries -= 1;
            } while (hasAnyProcess() && tries > 0);

            return tries > 0;
        }


        public static bool TryKillWord()
        {
            const string wordProcName = "WINWORD";
            try
            {
                Kill(wordProcName);
            }
            catch (Win32Exception)
            {
                // Win32Exception with an "Access is denied" message could occur when the process is already exiting.
                // We ignore this exception but return a boolean value to indicate if any Word processes exist.
                // If there are remaining processes, it means the kill operation failed, and the caller should act accordingly.
                // If there is none, it means the kill operation likely succeeded.
                // Tt could also mean that a Win32Exception had occurred, and that the process is already in exit state before the kill operation started.
                // In this case, Word process should exit anyway even though an exception is thrown.
            }

            return !Process.GetProcessesByName(wordProcName).Any();
        }

        public static void Kill(string procName)
        {
            var procs = Process.GetProcessesByName(procName);
            foreach (var tmp in procs)
            {
                tmp.Kill();
            }
        }

        public static Process[] GetProcessByWindowTitle(string title)
        {
            var processes = Process.GetProcesses();

            return processes.Where(process => process.MainWindowTitle.Equals(title)).ToArray();
        }

        public static void KillByWindowTitle(string title)
        {
            var processes = GetProcessByWindowTitle(title);

            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch (Win32Exception)
                {

                }
            }
        }

        public static bool TryKillByWindowTitle(string title)
        {
            KillByWindowTitle(title);

            return !GetProcessByWindowTitle(title).Any();
        }

        public static bool WaitForTitleProcessesToExit(string title, int waitTimeMillis = 30000)
        {
            return WaitForProcessesToExit(() => GetProcessByWindowTitle(title).Any(), waitTimeMillis);
        }
    }
}
