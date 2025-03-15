using ReciboFacil.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public interface IProdutoAplicacao
    {
        Task<int> CadastrarAsync(Produto produtoDTO);
        Task AtualizarAsync(Produto produtoDTO);
        Task DeletarAsync(int produtoId);
        Task<IEnumerable<Produto>> ListarAsync(bool ativo = true);
        Task<IEnumerable<Produto>> ListarProdutosPorClienteIdAsync(int clienteId);
        Task<Produto> ObterPorIdAsync(int id); 
         Task<List<Produto>> ListarTop10ProdutosAsync();
         Task<List<ProdutoPorCliente>> ListarProdutosPorClienteAsync(int clienteId);
    }
}