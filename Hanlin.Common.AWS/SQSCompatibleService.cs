using System;
using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Hanlin.Common.AWS
{
  
    public class SQSCompatibleService : ISQSService, IDisposable
    {
        public IAmazonSQS SQS { get; private set; }
        public string QueueName { get; private set; }
        protected string _queryUrl { get; set; }
        public string ServiceName { get; protected set; }

        public SQSCompatibleService(string serviceUrl, string accessKey, string secretKey, string queueName)
        {
            ServiceName = "";

            var amazonSQSConfig = new AmazonSQSConfig
            {
                ServiceURL = serviceUrl
            };

            SQS = Amazon.AWSClientFactory.CreateAmazonSQSClient(accessKey, secretKey, amazonSQSConfig);
            QueueName = queueName;

            CreateQueue();
        }

        private void CreateQueue()
        {
            var createQueueRequest = new CreateQueueRequest {QueueName = QueueName};
            var createQueueResponse = SQS.CreateQueue(createQueueRequest);

            if (createQueueResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                _queryUrl = createQueueResponse.QueueUrl;
            }
        }

        public string Send(string body)
        {
            var sendMsgRequest = new SendMessageRequest
            {
                QueueUrl = _queryUrl,
                MessageBody = body
            };

            var sendMessageResponse = SQS.SendMessage(sendMsgRequest);
            return sendMessageResponse.HttpStatusCode == HttpStatusCode.OK ? sendMessageResponse.MessageId : null;
        }

        public SQSMessage Get(bool delete = true, int? waitTimeSeconds = null, int? visibilityTimeout = null)
        {
            var receiveMessageRequest = new ReceiveMessageRequest()
            {
                QueueUrl = _queryUrl,
                MaxNumberOfMessages = 1
            };

            if (waitTimeSeconds.HasValue) receiveMessageRequest.WaitTimeSeconds = waitTimeSeconds.Value;

            if (visibilityTimeout.HasValue) receiveMessageRequest.VisibilityTimeout = visibilityTimeout.Value;

            var receiveMessageResponse = SQS.ReceiveMessage(receiveMessageRequest);
            var messages = receiveMessageResponse.Messages;

            if (messages.Count != 0)
            {
                var message = messages[0];
                var body = message.Body;

                if (delete) Delete(message.ReceiptHandle);

                return new SQSMessage
                {
                    Body = body,
                    MessageId = message.MessageId,
                    ReceiptHandle = message.ReceiptHandle
                };
            }

            return null;
        }

        public bool Delete(string receiptHandle)
        {
            var deleteMessageRequest = new DeleteMessageRequest()
            {
                QueueUrl = _queryUrl,
                ReceiptHandle = receiptHandle
            };

            var response = SQS.DeleteMessage(deleteMessageRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public void Dispose()
        {
            if (SQS != null)
            {
                SQS.Dispose();
            }
        }
    }
}