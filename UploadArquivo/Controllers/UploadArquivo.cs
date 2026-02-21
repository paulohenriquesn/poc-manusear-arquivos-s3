using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UploadArquivo.Services;

namespace UploadArquivo.Controllers
{

    public record DownloadRequest(string fileName);

    [ApiController]
    [Route("[controller]")]
    public class UploadArquivo : ControllerBase
    {
        private readonly ILogger<UploadArquivo> _logger;
        public UploadArquivo(ILogger<UploadArquivo> logger)
        {
            _logger = logger;
        }

        [HttpPost("download")]
        public async Task<IActionResult> Download([FromServices] IStorageService storageService, [FromBody] DownloadRequest body)
        {
            var url = await storageService.GetPresignedUrlAsync($"processamento/{body.fileName}", "paulohenriquesn");

            return Ok(new 
            {
                Url = url
            });
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IStorageService storageService,
            [FromServices] IQueueService queueService, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Esta faltando o arquivo");
            }

            await storageService.SaveOnStorageAsync($"processamento/{file.FileName}", "paulohenriquesn", file);

            var messageBody = JsonSerializer.Serialize(new
            {
                BucketName = "paulohenriquesn",
                FilePath = $"processamento/{file.FileName}"
            });

            await queueService.PublicOnTopic("processar-arquivos", messageBody);

            return Ok();
        }
    }
}
