using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReciboFacil.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Repositorio
{
    public class ProdutoRepositorio : BaseRepositorio, IProdutoRepositorio
    {

        public ProdutoRepositorio(ReciboFacilContexto contexto) : base(contexto)
        {

        }

        public async Task<int> CadastrarAsync(Produto produto)
        {
            await _contexto.Produtos.AddAsync(produto);
            await _contexto.SaveChangesAsync();
            return produto.Id;
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _contexto.Produtos.Update(produto);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var produto = await _contexto.Produtos.FindAsync(id);

            if (produto == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            produto.Deletar(); // Marca o produto como inativo (soft delete)
            _contexto.Produtos.Update(produto); // Atualiza o produto no banco de dados
            await _contexto.SaveChangesAsync(); // Salva as alterações
        }

        public async Task<List<Produto>> ListarAsync(bool ativo = true)
        {
            return await _contexto.Produtos
                .Where(p => p.Ativo == ativo) // Filtra por produtos ativos ou inativos
                .ToListAsync();
        }

        public async Task<List<Produto>> ListarProdutosPorClienteIdAsync(int clienteId)
        {
            return await _contexto.Produtos
                .Where(p => p.ClienteId == clienteId && p.Ativo)
                .ToListAsync();
        }

        public async Task<Produto> ObterPorIdAsync(int id)
        {
            return await _contexto.Produtos.FindAsync(id);
        }
        public async Task<List<Produto>> ListarTop10ProdutosAsync()
        {
            return await _contexto.Produtos
                .FromSqlRaw("EXEC ListarTop10Produtos")  // Chama a stored procedure
                .ToListAsync();
        }
        public async Task<List<ProdutoPorCliente>> ListarProdutosPorClienteAsync(int clienteId)
        {
            return await _contexto.ProdutosPorCliente
                .FromSqlRaw("SELECT * FROM dbo.ListarProdutosPorCliente({0})", clienteId)
                .ToListAsync();
        }
        public async Task<(List<Produto> produtos, int totalRegistros, int totalPaginas)> ListarPaginadoAsync(
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
            var produtos = await _contexto.Produtos
                .FromSqlRaw("EXEC sp_ListarProdutosAtivosPaginados @Pagina, @ItensPorPagina, @TotalRegistros OUTPUT, @TotalPaginas OUTPUT",
                    paginaParam, itensPorPaginaParam, totalRegistrosParam, totalPaginasParam)
                .ToListAsync();

            // Obter os valores de output
            var totalRegistros = (int)totalRegistrosParam.Value;
            var totalPaginas = (int)totalPaginasParam.Value;

            return (produtos, totalRegistros, totalPaginas);
        }
    }
}
