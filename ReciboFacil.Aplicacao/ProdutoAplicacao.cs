using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public class ProdutoAplicacao : IProdutoAplicacao
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoAplicacao(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<int> CadastrarAsync(Produto produtoDTO)
        {
            if (produtoDTO == null)
            {
                throw new Exception("Produto não pode ser vazio");
            }

            return await _produtoRepositorio.CadastrarAsync(produtoDTO);
        }

        public async Task AtualizarAsync(Produto produtoDTO)
        {
            var produtoExistente = await _produtoRepositorio.ObterPorIdAsync(produtoDTO.Id);

            if (produtoExistente == null)
            {
                throw new Exception("Produto não encontrado");
            }

            produtoExistente.Nome = produtoDTO.Nome;
            produtoExistente.Marca = produtoDTO.Marca;
            produtoExistente.Modelo = produtoDTO.Modelo;
            produtoExistente.Preco = produtoDTO.Preco;

            await _produtoRepositorio.AtualizarAsync(produtoExistente);
        }

        public async Task DeletarAsync(int produtoId)
        {
            var produto = await _produtoRepositorio.ObterPorIdAsync(produtoId);

            if (produto == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            produto.Deletar(); // Marca o produto como inativo (soft delete)
            await _produtoRepositorio.AtualizarAsync(produto); // Atualiza o produto no banco de dados
        }

        public async Task<Produto> ObterPorIdAsync(int id)
        {
            return await _produtoRepositorio.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<Produto>> ListarAsync(bool ativo = true)
        {
            return await _produtoRepositorio.ListarAsync(ativo);
        }

        public async Task<IEnumerable<Produto>> ListarProdutosPorClienteIdAsync(int clienteId)
        {
            return await _produtoRepositorio.ListarProdutosPorClienteIdAsync(clienteId);
        }
        public async Task<List<Produto>> ListarTop10ProdutosAsync()
        {
            return await _produtoRepositorio.ListarTop10ProdutosAsync();
        }
        public async Task<List<ProdutoPorCliente>> ListarProdutosPorClienteAsync(int clienteId)
        {
            return await _produtoRepositorio.ListarProdutosPorClienteAsync(clienteId);
        }
        public async Task<(List<Produto> produtos, int totalRegistros, int totalPaginas)> ListarPaginadoAsync(
            int pagina = 1,
            int itensPorPagina = 10)
        {
            return await _produtoRepositorio.ListarPaginadoAsync(pagina, itensPorPagina);
        }
    }
}