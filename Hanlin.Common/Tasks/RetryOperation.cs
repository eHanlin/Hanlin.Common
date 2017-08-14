using System;
using System.Reflection;
using log4net;

namespace Hanlin.Common.Tasks
{
    public class RetryOperation
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Action Operation { get; }
        public int MaxAttempts { get; }
        public int AttemptCount { get; private set; }

        public RetryOperation(Action operation, int times = 3)
        {
            Operation = operation;
            MaxAttempts = times;
        }

        public void Execute()
        {
            Execute<Exception>();
        }

        public void Execute<TEx>() where TEx : Exception
        {
            AttemptCount = 1;

            while (true)
            {
                try
                {
                    Operation();
                    break;
                }
                catch (TEx e)
                {
                    Log.Warn($"Attempt {AttemptCount}/{MaxAttempts} resulted in exception: {e}");

                    if (AttemptCount == MaxAttempts)
                    {
                        Log.Warn($"Retry attempts exhausted ({MaxAttempts} attempts were made.)");

                        throw;
                    }
                    else
                    {
                        AttemptCount += 1;
                    }
                }
            }
        }
    }
}
