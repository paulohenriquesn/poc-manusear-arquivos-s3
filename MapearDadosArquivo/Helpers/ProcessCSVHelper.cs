namespace MapearDadosArquivo.Helpers
{
    public class ProcessCSVHelper
    {
        public List<DadosMapeados> Execute(StreamReader stream)
        {

           List<DadosMapeados> data = new List<DadosMapeados>();

           while(!stream.EndOfStream)
            {
                var line = stream.ReadLine();
                var values = line.Split(',');

                if (values[0] != "cpf")
                {
                    data.Add(new DadosMapeados
                    {
                        CPF = values[0].Trim(),
                        CNPJ = values[1].Trim(),
                        Amount = double.Parse(values[2].Trim())
                    });
                }
            }

            return data;
        }
    }
}
    