using Amazon.SQS;
using Amazon.SQS.Model;

namespace UploadArquivo.Services
{
    public class QueueService : IQueueService
    {
        private readonly AmazonSQSClient _client;

        public QueueService()
        {
            _client = new AmazonSQSClient(Amazon.RegionEndpoint.SAEast1);
        }

        public async Task PublicOnTopic(string topicName, string messageBody)
        {

            var getQueueResponse = await _client.GetQueueUrlAsync(topicName);

            var request = new SendMessageRequest
            {
                QueueUrl = getQueueResponse.QueueUrl,
                MessageBody = messageBody
            };

            await _client.SendMessageAsync(request);
        }
    }
}
