using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReciboFacil.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Repositorio
{
    public class ClienteRepositorio : BaseRepositorio, IClienteRepositorio
    {
        public ClienteRepositorio(ReciboFacilContexto contexto) : base(contexto)
        {
        }

        public async Task<int> CadastrarAsync(Cliente cliente)
        {
            await _contexto.Clientes.AddAsync(cliente);
            await _contexto.SaveChangesAsync();
            return cliente.Id;
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _contexto.Clientes.Update(cliente);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var cliente = await _contexto.Clientes.FindAsync(id);
            if (cliente != null)
            {
                cliente.Deletar();
                _contexto.Clientes.Update(cliente);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<List<Cliente>> ListarAsync(bool ativo)
        {
            return await _contexto.Clientes
                .Where(c => c.Ativo == ativo)
                .ToListAsync();
        }

        public async Task<Cliente> ObterPorIdAsync(int id)
        {
            return await _contexto.Clientes.FindAsync(id);
        }

        public async Task RestaurarAsync(int id)
        {
            var cliente = await _contexto.Clientes.FindAsync(id);
            if (cliente != null)
            {
                cliente.Restaurar();
                _contexto.Clientes.Update(cliente);
                await _contexto.SaveChangesAsync();
            }
        }
        public async Task<List<Cliente>> ListarTop10ClientesAsync()
        {
            return await _contexto.Clientes
                .FromSqlRaw("EXEC ListarTop10Clientes")  // Chama a stored procedure
                .ToListAsync();
        }
        public async Task<(List<Cliente> clientes, int totalRegistros, int totalPaginas)> ListarPaginadoAsync(
           int pagina = 1,
           int itensPorPagina = 10)
        {
            // Parâmetros para a stored procedure
            var paginaParam = new SqlParameter("@Pagina", pagina);
            var itensPorPaginaParam = new SqlParameter("@ItensPorPagina", itensPorPagina);

            var totalRegistrosParam = new SqlParameter
            {
                ParameterName = "@TotalRegistros",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var totalPaginasParam = new SqlParameter
            {
                ParameterName = "@TotalPaginas",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Executar a stored procedure
            var clientes = await _contexto.Clientes
                .FromSqlRaw("EXEC sp_ListarClientesAtivosPaginados @Pagina, @ItensPorPagina, @TotalRegistros OUTPUT, @TotalPaginas OUTPUT",
                    paginaParam, itensPorPaginaParam, totalRegistrosParam, totalPaginasParam)
                .ToListAsync();

            // Obter os valores de output
            var totalRegistros = (int)totalRegistrosParam.Value;
            var totalPaginas = (int)totalPaginasParam.Value;

            return (clientes, totalRegistros, totalPaginas);
        }
    }
}

