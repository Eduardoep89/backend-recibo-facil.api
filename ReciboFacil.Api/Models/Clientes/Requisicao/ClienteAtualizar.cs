namespace ReciboFacil.Api.Models.Clientes.Requisicao
{
    public class ClienteAtualizar
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string CnpjCpf { get; set; }
    }
}