using Microsoft.AspNetCore.Mvc;
using ReciboFacil.Aplicacao;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Api.Models.Produtos.Requisicao;
using ReciboFacil.Api.Models.Produtos.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoAplicacao _produtoAplicacao;

        public ProdutoController(IProdutoAplicacao produtoAplicacao)
        {
            _produtoAplicacao = produtoAplicacao;
        }

        [HttpPost("Cadastrar")]
        public async Task<ActionResult<int>> CadastrarAsync([FromBody] ProdutoCriar produtoCriar)
        {
            try
            {
                var produto = new Produto
                {
                    Nome = produtoCriar.Nome,
                    Marca = produtoCriar.Marca,
                    Modelo = produtoCriar.Modelo,
                    Preco = produtoCriar.Preco,
                    ClienteId = produtoCriar.ClienteId
                };

                var produtoId = await _produtoAplicacao.CadastrarAsync(produto);
                return Ok(produtoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar")]
        public async Task<ActionResult> AtualizarAsync([FromBody] ProdutoAtualizar produtoAtualizar)
        {
            try
            {
                var produto = new Produto
                {
                    Id = produtoAtualizar.Id,
                    Nome = produtoAtualizar.Nome,
                    Marca = produtoAtualizar.Marca,
                    Modelo = produtoAtualizar.Modelo,
                    Preco = produtoAtualizar.Preco,
                    ClienteId = produtoAtualizar.ClienteId
                };

                await _produtoAplicacao.AtualizarAsync(produto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Obter/{produtoId}")]
        public async Task<ActionResult<ProdutoResposta>> ObterPorIdAsync([FromRoute] int produtoId)
        {
            try
            {
                var produto = await _produtoAplicacao.ObterPorIdAsync(produtoId);

                if (produto == null)
                {
                    return NotFound("Produto não encontrado.");
                }

                var resposta = new ProdutoResposta
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Marca = produto.Marca,
                    Modelo = produto.Modelo,
                    Preco = produto.Preco,
                    Ativo = produto.Ativo,
                    ClienteId = produto.ClienteId
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Deletar/{produtoId}")]
        public async Task<ActionResult> DeletarAsync([FromRoute] int produtoId)
        {
            try
            {
                await _produtoAplicacao.DeletarAsync(produtoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<ProdutoResposta>>> ListarAsync([FromQuery] bool ativo = true)
        {
            try
            {
                var produtos = await _produtoAplicacao.ListarAsync(ativo);

                var resposta = produtos.Select(produto => new ProdutoResposta
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Marca = produto.Marca,
                    Modelo = produto.Modelo,
                    Preco = produto.Preco,
                    Ativo = produto.Ativo,
                    ClienteId = produto.ClienteId
                }).ToList();

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ListarTop10")]
        public async Task<ActionResult<IEnumerable<ProdutoResposta>>> ListarTop10ProdutosAsync()
        {
            try
            {
                var produtos = await _produtoAplicacao.ListarTop10ProdutosAsync();

                var resposta = produtos.Select(produto => new ProdutoResposta
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Marca = produto.Marca,
                    Modelo = produto.Modelo,
                    Preco = produto.Preco,
                    Ativo = produto.Ativo,
                    ClienteId = produto.ClienteId
                }).ToList();

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ListarPorCliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ProdutoPorCliente>>> ListarProdutosPorClienteAsync([FromRoute] int clienteId)
        {
            try
            {
                var produtos = await _produtoAplicacao.ListarProdutosPorClienteAsync(clienteId);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
