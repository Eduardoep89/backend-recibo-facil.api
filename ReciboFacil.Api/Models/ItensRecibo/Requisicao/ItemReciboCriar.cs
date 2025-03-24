namespace ReciboFacil.Api.Models.ItensRecibo.Requisicao
{
    public class ItemReciboCriar
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}