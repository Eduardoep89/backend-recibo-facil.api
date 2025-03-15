using Microsoft.AspNetCore.Mvc;
using ReciboFacil.Aplicacao;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Api.Models.ItensRecibo.Requisicao;
using ReciboFacil.Api.Models.ItensRecibo.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemReciboController : ControllerBase
    {
        private readonly IItemReciboAplicacao _itemReciboAplicacao;

        public ItemReciboController(IItemReciboAplicacao itemReciboAplicacao)
        {
            _itemReciboAplicacao = itemReciboAplicacao;
        }

        [HttpPost("Cadastrar")]
        public async Task<ActionResult<int>> CadastrarAsync([FromBody] ItemReciboCriar itemReciboCriar)
        {
            try
            {
                var itemRecibo = new ItemRecibo
                {
                    ProdutoId = itemReciboCriar.ProdutoId,
                    Quantidade = itemReciboCriar.Quantidade,
                    PrecoUnitario = itemReciboCriar.PrecoUnitario,
                };

                var itemReciboId = await _itemReciboAplicacao.CadastrarAsync(itemRecibo);
                return Ok(itemReciboId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar")]
        public async Task<ActionResult> AtualizarAsync([FromBody] ItemReciboAtualizar itemReciboAtualizar)
        {
            try
            {
                var itemRecibo = new ItemRecibo
                {
                    Id = itemReciboAtualizar.Id,
                    ProdutoId = itemReciboAtualizar.ProdutoId,
                    Quantidade = itemReciboAtualizar.Quantidade,
                    PrecoUnitario = itemReciboAtualizar.PrecoUnitario
                };

                await _itemReciboAplicacao.AtualizarAsync(itemRecibo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Obter/{itemReciboId}")]
        public async Task<ActionResult<ItemReciboResposta>> ObterPorIdAsync([FromRoute] int itemReciboId)
        {
            try
            {
                var itemRecibo = await _itemReciboAplicacao.ObterPorIdAsync(itemReciboId);

                if (itemRecibo == null)
                {
                    return NotFound("Item de recibo não encontrado.");
                }

                var resposta = new ItemReciboResposta
                {
                    Id = itemRecibo.Id,
                    ProdutoId = itemRecibo.ProdutoId,
                    Quantidade = itemRecibo.Quantidade,
                    PrecoUnitario = itemRecibo.PrecoUnitario,
                    Subtotal = itemRecibo.Subtotal
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Deletar/{itemReciboId}")]
        public async Task<ActionResult> DeletarAsync([FromRoute] int itemReciboId)
        {
            try
            {
                await _itemReciboAplicacao.DeletarAsync(itemReciboId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarPorRecibo/{reciboId}")]
        public async Task<ActionResult<IEnumerable<ItemReciboResposta>>> ListarPorReciboIdAsync([FromRoute] int reciboId)
        {
            try
            {
                var itensRecibo = await _itemReciboAplicacao.ListarPorReciboIdAsync(reciboId);

                var resposta = itensRecibo.Select(item => new ItemReciboResposta
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    Subtotal = item.Subtotal
                }).ToList();

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}