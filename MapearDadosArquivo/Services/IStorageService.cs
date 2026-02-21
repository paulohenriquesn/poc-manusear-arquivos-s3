namespace MapearDadosArquivo.Services
{
    internal interface IStorageService
    {
        Task<Stream> GetFileAsync(string bucketName, string Path);
        Task SaveFileAsync(MemoryStream fileStream, string bucketName, string Path);
    }
}
