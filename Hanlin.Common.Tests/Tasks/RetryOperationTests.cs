using System;
using Hanlin.Common.Tasks;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Tasks
{
    class RetryOperationTests
    {
        [Test]
        public void Retry_Succeeds()
        {
            var op = new RetryOperation(() => { });
            op.Execute();

            Assert.AreEqual(1, op.AttemptCount);
        }

        [Test]
        public void Retry_Fails()
        {
            var op = new RetryOperation(() => { throw new Exception(); });

            Assert.Throws<Exception>(() => op.Execute());
            Assert.AreEqual(op.MaxAttempts, op.AttemptCount);
        }
    }
}
