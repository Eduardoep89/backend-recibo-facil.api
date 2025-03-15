namespace ReciboFacil.Api.Models.Produtos.Requisicao
{
    public class ProdutoAtualizar
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public decimal Preco { get; set; }
        public int ClienteId { get; set; } // Relacionamento com o cliente
    }
}