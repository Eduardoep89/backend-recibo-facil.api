namespace ReciboFacil.Api.Models.Relatorios
{
    public class SugestoesResposta
    {
        public string Sugestoes { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
    }
}