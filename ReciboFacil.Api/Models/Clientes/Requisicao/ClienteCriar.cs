namespace ReciboFacil.Api.Models.Clientes.Requisicao
{
    public class ClienteCriar
    {
         public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string CnpjCpf { get; set; }
    }
}