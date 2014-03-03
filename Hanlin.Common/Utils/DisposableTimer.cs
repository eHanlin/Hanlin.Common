using System;
using System.Diagnostics;
using System.Reflection;
using log4net;

namespace Hanlin.Common.Utils
{
    public class DisposableTimer : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static int IndentLevel { get; set; }
        public static int IndentSize { get; set; }
        public static char IndentChar { get; set; }

        static DisposableTimer()
	    {
            IndentChar = ' ';
            IndentSize = 4;
	    }

        private int currentIndent = 0;
        private string task;
        private Stopwatch timer = new Stopwatch();

        public string Task 
        {
            get { return this.task; }
        }

        public DisposableTimer(string task = "Operation")
            : this(task, new string[0])
        { }

        public DisposableTimer(string task, params string[] msgs)
        {
            SaveIndentLevel();
            this.task = task;
            LogIndented(">" + Task);
            foreach (var m in msgs)
            {
                LogIndented(m);
            }
            timer.Start();
        }

        private void SaveIndentLevel()
        {
            currentIndent = DisposableTimer.IndentLevel;
            IndentLevel++;
        }

        public void Dispose()
        {
            timer.Stop();
            TimeSpan elapsed = timer.Elapsed;
            LogIndented(string.Format("[{0:D2}m {1:D2}s {2:D3}ms]", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds));
            IndentLevel--;
        }

        private void LogIndented(string msg)
        {
            Log.Info(IndentString + msg);
        }

        private string IndentString
        {
            get { return new string(IndentChar, currentIndent * IndentSize); }
        }
    }
}
