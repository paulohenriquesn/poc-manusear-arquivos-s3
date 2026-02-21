namespace MapearDadosArquivo
{
    public record FunctionRequest
    {
        public string BucketName { get; set; }
        public string FilePath { get; set; }
    }
}
