using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace MapearDadosArquivo.Services
{
    internal class StorageService : IStorageService
    {
        private readonly AmazonS3Client _client;

        public StorageService()
        {
            _client = new AmazonS3Client(Amazon.RegionEndpoint.SAEast1);
        }

        public async Task<Stream> GetFileAsync(string bucketName, string Path)
        {

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = Path,
            };

            var response = await _client.GetObjectAsync(request);

            return response.ResponseStream;
        }

        public Task SaveFileAsync(MemoryStream fileStream, string bucketName, string Path)
        {

            var ObjectRequest = new TransferUtilityUploadRequest
            {
                ContentType = "application/json",
                BucketName = bucketName,
                Key = Path,
                InputStream = fileStream,
            };

            using (var transferUtility = new TransferUtility(_client))
            {
                return transferUtility.UploadAsync(ObjectRequest);
            }
        }
    }
}
