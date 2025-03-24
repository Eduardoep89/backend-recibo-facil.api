namespace ReciboFacil.Api.Models.Produtos.Resposta
{
    public class ProdutoResposta
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public int ClienteId { get; set; } // Relacionamento com o cliente
    }
}