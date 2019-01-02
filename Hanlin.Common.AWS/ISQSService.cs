namespace Hanlin.Common.AWS
{
    public class SQSMessage
    {
        public string Body { get; set; }
        public string MessageId { get; set; }
        public string ReceiptHandle { get; set; }
    }

    public interface ISQSService
    {
        string Send(string body);
        SQSMessage Get(bool delete = true, int? waitTimeSeconds = null, int? visibilityTimeout = null);
        bool Delete(string receiptHandle);
        void Dispose();
    }
}