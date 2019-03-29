using System;
using System.Threading;

namespace Hanlin.Common
{
    public class CountLocker
    {
        public object Locker { get; } = new Object();
        private int Timout { get; set; }
        private readonly Func<bool> _stop;
        private readonly Func<object> _done;
        private int _count = 0;
        private Thread _thread;

        public CountLocker(Func<bool> stop, Func<object> done, int timeout)
        {
            Timout = timeout;
            _stop = stop;
            _done = done;
            _count = 0;
        }

        public void Start()
        {
            if (_thread != null) return;
            _thread = new Thread(Release);
            _thread.Start();
        }

        public void Inc()
        {
            _count++;
        }

        public void Desc()
        {
            _count--;
        }

        private void Release()
        {
            var stop = true;

            do
            {
                Thread.Sleep(Timout * 2);

                lock (Locker)
                {
                    Thread.Sleep(Timout / 120);

                    if (_count > 0 || !_stop())
                    {
                        stop = false;
                    }
                    else
                    {
                        stop = true;
                    }
                }

            } while (!stop);

            _done();
        }
    }
}