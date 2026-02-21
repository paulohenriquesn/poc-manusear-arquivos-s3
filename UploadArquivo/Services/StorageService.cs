using Amazon.S3;
using Amazon.S3.Transfer;

namespace UploadArquivo.Services
{
    public class StorageService : IStorageService
    {
        private readonly ILogger<IStorageService> _logger;
        private IAmazonS3 client;


        public StorageService(ILogger<IStorageService> logger)
        {
            _logger = logger;
            client = new AmazonS3Client(Amazon.RegionEndpoint.SAEast1);
        }

        public async Task<string> GetPresignedUrlAsync(string filePath, string bucketName)
        {
            var presignedUrlRequest = new Amazon.S3.Model.GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = filePath,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };

            return await client.GetPreSignedURLAsync(presignedUrlRequest);
        }

        public async Task SaveOnStorageAsync(string filePath, string bucketName, IFormFile file)
        {
            var transferUtility = new TransferUtility(client);

            _logger.LogInformation("Salvando arquivo no storage.");

            using (var fileStream = file.OpenReadStream())
            {
                await transferUtility.UploadAsync(new TransferUtilityUploadRequest {
                    InputStream = fileStream,
                    BucketName = bucketName,
                    Key = filePath,
                    ContentType = file.ContentType
                });
            }
        }
    }
}