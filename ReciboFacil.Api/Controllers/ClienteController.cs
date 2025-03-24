using Microsoft.AspNetCore.Mvc;
using ReciboFacil.Aplicacao;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Api.Models.Clientes.Requisicao;
using ReciboFacil.Api.Models.Clientes.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteAplicacao _clienteAplicacao;

        public ClienteController(IClienteAplicacao clienteAplicacao)
        {
            _clienteAplicacao = clienteAplicacao;
        }

        [HttpPost("Cadastrar")]
        public async Task<ActionResult<int>> CadastrarAsync([FromBody] ClienteCriar clienteCriar)
        {
            try
            {
                var cliente = new Cliente
                {
                    Nome = clienteCriar.Nome,
                    Endereco = clienteCriar.Endereco,
                    Bairro = clienteCriar.Bairro,
                    Cidade = clienteCriar.Cidade,
                    Telefone = clienteCriar.Telefone,
                    CnpjCpf = clienteCriar.CnpjCpf
                };

                var clienteId = await _clienteAplicacao.CadastrarAsync(cliente);
                return Ok(clienteId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar")]
        public async Task<ActionResult> AtualizarAsync([FromBody] ClienteAtualizar clienteAtualizar)
        {
            try
            {
                var cliente = new Cliente
                {
                    Id = clienteAtualizar.Id,
                    Nome = clienteAtualizar.Nome,
                    Endereco = clienteAtualizar.Endereco,
                    Bairro = clienteAtualizar.Bairro,
                    Cidade = clienteAtualizar.Cidade,
                    Telefone = clienteAtualizar.Telefone,
                    CnpjCpf = clienteAtualizar.CnpjCpf
                };

                await _clienteAplicacao.AtualizarAsync(cliente);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Obter/{clienteId}")]
        public async Task<ActionResult<ClienteResposta>> ObterPorIdAsync([FromRoute] int clienteId)
        {
            try
            {
                var cliente = await _clienteAplicacao.ObterPorIdAsync(clienteId);

                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado.");
                }

                var resposta = new ClienteResposta
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Endereco = cliente.Endereco,
                    Bairro = cliente.Bairro,
                    Cidade = cliente.Cidade,
                    Telefone = cliente.Telefone,
                    CnpjCpf = cliente.CnpjCpf,
                    Ativo = cliente.Ativo
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Deletar/{clienteId}")]
public async Task<ActionResult> DeletarAsync([FromRoute] int clienteId)
{
    try
    {
        await _clienteAplicacao.DeletarAsync(clienteId);
        return Ok();
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
[HttpGet("Listar")]
public async Task<ActionResult<IEnumerable<ClienteResposta>>> ListarAsync([FromQuery] bool ativo = true)
{
    try
    {
        var clientes = await _clienteAplicacao.ListarAsync(ativo);

        var resposta = clientes.Select(cliente => new ClienteResposta
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Endereco = cliente.Endereco,
            Bairro = cliente.Bairro,
            Cidade = cliente.Cidade,
            Telefone = cliente.Telefone,
            CnpjCpf = cliente.CnpjCpf,
            Ativo = cliente.Ativo
        }).ToList();

        return Ok(resposta);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
[HttpGet("ListarTop10")]
public async Task<ActionResult<IEnumerable<ClienteResposta>>> ListarTop10ClientesAsync()
{
    try
    {
        var clientes = await _clienteAplicacao.ListarTop10ClientesAsync();

        var resposta = clientes.Select(cliente => new ClienteResposta
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Endereco = cliente.Endereco,
            Bairro = cliente.Bairro,
            Cidade = cliente.Cidade,
            Telefone = cliente.Telefone,
            CnpjCpf = cliente.CnpjCpf,
            Ativo = cliente.Ativo
        }).ToList();

        return Ok(resposta);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

        // Outros métodos...
    }
}