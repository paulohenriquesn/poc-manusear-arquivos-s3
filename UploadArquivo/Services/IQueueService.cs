namespace UploadArquivo.Services
{
    public interface IQueueService
    {
        Task PublicOnTopic(string topicName, string messageBody);
    }
}
