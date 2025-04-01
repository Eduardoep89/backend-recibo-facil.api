namespace ReciboFacil.Api.Models.PagedResult

{
    public class PagedResult<T>
    {
        public int PaginaAtual { get; set; }
        public int ItensPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Itens { get; set; }
    }
}