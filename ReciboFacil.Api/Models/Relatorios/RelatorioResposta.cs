namespace ReciboFacil.Api.Models.Relatorios
{
    public class RelatorioResposta
    {
        public string Relatorio { get; set; }
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
    }
}
