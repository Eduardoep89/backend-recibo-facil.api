using Microsoft.AspNetCore.Mvc;
using ReciboFacil.Aplicacao;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Api.Models.Recibos.Requisicao;
using ReciboFacil.Api.Models.Recibos.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReciboFacil.Api.Models.ItensRecibo.Resposta;

namespace ReciboFacil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReciboController : ControllerBase
    {
        private readonly IReciboAplicacao _reciboAplicacao;

        public ReciboController(IReciboAplicacao reciboAplicacao)
        {
            _reciboAplicacao = reciboAplicacao;
        }

      [HttpPost("Cadastrar")]
public async Task<ActionResult<int>> CadastrarAsync([FromBody] ReciboCriar reciboCriar)
{
    try
    {
        var recibo = new Recibo
        {
            NumeroPedido = reciboCriar.NumeroPedido,
            Data = reciboCriar.Data,
            Descricao = reciboCriar.Descricao,
            ClienteId = reciboCriar.ClienteId,
            Itens = reciboCriar.Itens.Select(item => new ItemRecibo
            {
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario
            }).ToList()
        };

        // Calcular o subtotal de cada item
        foreach (var item in recibo.Itens)
        {
            item.CalcularSubtotal();
        }

        // Calcular o subtotal e o total do recibo
        recibo.CalcularTotais();

        var reciboId = await _reciboAplicacao.CadastrarAsync(recibo);
        return Ok(reciboId);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

        [HttpPut("Atualizar")]
public async Task<ActionResult> AtualizarAsync([FromBody] ReciboAtualizar reciboAtualizar)
{
    try
    {
        var recibo = new Recibo
        {
            Id = reciboAtualizar.Id,
            NumeroPedido = reciboAtualizar.NumeroPedido,
            Data = reciboAtualizar.Data,
            Descricao = reciboAtualizar.Descricao,
            ClienteId = reciboAtualizar.ClienteId,
            Itens = reciboAtualizar.Itens.Select(item => new ItemRecibo
            {
                Id = item.Id,
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario
            }).ToList()
        };

        // Calcular o subtotal de cada item
        foreach (var item in recibo.Itens)
        {
            item.CalcularSubtotal();
        }

        // Calcular o subtotal e o total do recibo
        recibo.CalcularTotais();

        await _reciboAplicacao.AtualizarAsync(recibo);
        return Ok();
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
      [HttpGet("Obter/{reciboId}")]
public async Task<ActionResult<ReciboResposta>> ObterPorIdAsync([FromRoute] int reciboId)
{
    try
    {
        var recibo = await _reciboAplicacao.ObterPorIdAsync(reciboId);

        if (recibo == null)
        {
            return NotFound("Recibo não encontrado.");
        }

        var resposta = new ReciboResposta
        {
            Id = recibo.Id,
            NumeroPedido = recibo.NumeroPedido,
            Data = recibo.Data,
            Descricao = recibo.Descricao,
            Subtotal = recibo.Subtotal,
            Total = recibo.Total,
            Ativo = recibo.Ativo,
            ClienteId = recibo.ClienteId,
            Itens = recibo.Itens.Select(item => new ItemReciboResposta
            {
                Id = item.Id,
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario,
                Subtotal = item.Subtotal
            }).ToList()
        };

        return Ok(resposta);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

        [HttpDelete("Deletar/{reciboId}")]
        public async Task<ActionResult> DeletarAsync([FromRoute] int reciboId)
        {
            try
            {
                await _reciboAplicacao.DeletarAsync(reciboId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<ReciboResposta>>> ListarAsync([FromQuery] bool ativo = true)
        {
            try
            {
                var recibos = await _reciboAplicacao.ListarAsync(ativo);

                var resposta = recibos.Select(recibo => new ReciboResposta
                {
                    Id = recibo.Id,
                    NumeroPedido = recibo.NumeroPedido,
                    Data = recibo.Data,
                    Descricao = recibo.Descricao,
                    Subtotal = recibo.Subtotal,
                    Total = recibo.Total,
                    Ativo = recibo.Ativo,
                    ClienteId = recibo.ClienteId,
                    Itens = recibo.Itens.Select(item => new ItemReciboResposta
                    {
                        Id = item.Id,
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario,
                        Subtotal = item.Subtotal
                    }).ToList()
                }).ToList();

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarTop10")]
        public async Task<ActionResult<IEnumerable<ReciboResposta>>> ListarTop10RecibosAsync()
        {
            try
            {
                var recibos = await _reciboAplicacao.ListarTop10RecibosAsync();

                var resposta = recibos.Select(recibo => new ReciboResposta
                {
                    Id = recibo.Id,
                    NumeroPedido = recibo.NumeroPedido,
                    Data = recibo.Data,
                    Descricao = recibo.Descricao,
                    Subtotal = recibo.Subtotal,
                    Total = recibo.Total,
                    Ativo = recibo.Ativo,
                    ClienteId = recibo.ClienteId,
                    Itens = recibo.Itens.Select(item => new ItemReciboResposta
                    {
                        Id = item.Id,
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario,
                        Subtotal = item.Subtotal
                    }).ToList()
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