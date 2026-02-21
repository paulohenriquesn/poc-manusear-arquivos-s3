using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MapearDadosArquivo.Helpers;
using MapearDadosArquivo.Services;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MapearDadosArquivo
{
    public class Function
    {
        private readonly StorageService _storageService = new StorageService();

        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {

                try
                {
                    context.Logger.LogLine($"Received one message: {record.Body} processing...");
                    var input = JsonSerializer.Deserialize<FunctionRequest>(record.Body);

                    var fileStream = await _storageService.GetFileAsync(input.BucketName, input.FilePath);

                    var fileName = input.FilePath.Split('.')[0].Split('/')[1];

                    var csvHelper = new ProcessCSVHelper();

                    var streamReader = new StreamReader(fileStream);

                    var listOfMappedData = csvHelper.Execute(streamReader);

                    context.Logger.LogLine(JsonSerializer.Serialize(listOfMappedData));

                    using (var memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(listOfMappedData)));
                        await _storageService.SaveFileAsync(memoryStream, "paulohenriquesn", $"processamento/{fileName}.json");
                    }
                }

                catch (Exception exception)
                {
                    context.Logger.LogLine($"{exception.Message} {exception.StackTrace}");
                    throw;
                }
            }
        }
    }
}


