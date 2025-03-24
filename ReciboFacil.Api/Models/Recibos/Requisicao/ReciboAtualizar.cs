using ReciboFacil.Api.Models.ItensRecibo.Requisicao;

namespace ReciboFacil.Api.Models.Recibos.Requisicao
{
    public class ReciboAtualizar
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public int ClienteId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        public List<ItemReciboAtualizar> Itens { get; set; }
    }
}