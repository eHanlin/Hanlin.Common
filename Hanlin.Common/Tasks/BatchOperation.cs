using System;
using System.Collections.Generic;
using System.Linq;

namespace Hanlin.Common.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements of the input collection.</typeparam>
    /// <typeparam name="TR">The operation result type.</typeparam>
    public class BatchOperation<T, TR>
    {
        public int BatchSize { get; set; }

        public BatchOperation(int batchSize = 1000)
        {
            BatchSize = batchSize;
        }

        public void Execute(IReadOnlyCollection<T> input, Action<IReadOnlyCollection<T>, TR> operation, TR operationResults)
        {
            var numberOfBatches = (int) Math.Ceiling(input.Count / (double) BatchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batchInput = input.Skip(i * BatchSize).Take(BatchSize).ToArray();

                operation(batchInput, operationResults);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements of the input collection.</typeparam>
    public class BatchOperation<T>
    {
        public int BatchSize { get; set; }

        public BatchOperation(int batchSize = 1000)
        {
            BatchSize = batchSize;
        }

        public void Execute(IReadOnlyCollection<T> input, Action<IReadOnlyCollection<T>> operation)
        {
            var numberOfBatches = (int)Math.Ceiling(input.Count / (double)BatchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batchInput = input.Skip(i * BatchSize).Take(BatchSize).ToArray();

                operation(batchInput);
            }
        }
    }
}