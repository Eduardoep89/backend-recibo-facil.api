namespace ReciboFacil.Api.Models.ItensRecibo.Requisicao
{
    public class ItemReciboAtualizar
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
        
    }
}