using ReciboFacil.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Repositorio
{
    public interface IClienteRepositorio
    {
        Task<int> CadastrarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task DeletarAsync(int id);
        Task<List<Cliente>> ListarAsync(bool ativo);
        Task<List<Cliente>> ListarTop10ClientesAsync();
        Task<Cliente> ObterPorIdAsync(int id);
        Task RestaurarAsync(int id);
        Task<(List<Cliente> clientes, int totalRegistros, int totalPaginas)> ListarPaginadoAsync(
            int pagina = 1,
            int itensPorPagina = 10);
    }
}