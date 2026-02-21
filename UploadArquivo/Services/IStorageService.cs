namespace UploadArquivo.Services
{
    public interface IStorageService
    {
        Task SaveOnStorageAsync(string filePath, string bucketName, IFormFile file);
        Task<string> GetPresignedUrlAsync(string filePath, string bucketName);
    }
}
