using ReciboFacil.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProdutoRepositorio
{
    Task<int> CadastrarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task DeletarAsync(int id);
    Task<List<Produto>> ListarAsync(bool ativo = true);
    Task<List<Produto>> ListarTop10ProdutosAsync();
    Task<List<Produto>> ListarProdutosPorClienteIdAsync(int clienteId);
    Task<Produto> ObterPorIdAsync(int id);
    Task<List<ProdutoPorCliente>> ListarProdutosPorClienteAsync(int clienteId);
}
