namespace Hanlin.Common.AWS
{
    public class AmazonSQSService : SQSCompatibleService
    {
        public AmazonSQSService(string serviceUrl, string accessKey, string secretKey, string queueName) : base(serviceUrl, accessKey, secretKey, queueName)
        {
            ServiceName = "AmazonSQS";
        }
    }
}