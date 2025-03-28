using System;
using System.Threading.Tasks;

namespace ReciboFacil.Servicos.Interfaces
{
    public interface IAIService
    {
        Task<string> GerarRelatorioAnaliticoAsync();
        Task<string> GerarSugestoesProdutosAsync(int clienteId);
        Task<string> GerarRelatorioPersonalizadoAsync(string promptPersonalizado);
    }
}