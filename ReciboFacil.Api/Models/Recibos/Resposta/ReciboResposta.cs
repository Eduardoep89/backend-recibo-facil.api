using ReciboFacil.Api.Models.ItensRecibo.Resposta;

namespace ReciboFacil.Api.Models.Recibos.Resposta
{
    public class ReciboResposta
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public bool Ativo { get; set; }
        public int ClienteId { get; set; }
        public List<ItemReciboResposta> Itens { get; set; }
    }
}