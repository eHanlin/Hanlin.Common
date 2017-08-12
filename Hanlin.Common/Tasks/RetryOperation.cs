using System;

namespace Hanlin.Common.Tasks
{
    public class RetryOperation
    {
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
                catch (TEx)
                {
                    if (AttemptCount == MaxAttempts)
                    {
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
