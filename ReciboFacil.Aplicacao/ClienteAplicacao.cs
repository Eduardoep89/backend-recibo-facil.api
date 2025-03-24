using ReciboFacil.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public class ClienteAplicacao : IClienteAplicacao
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteAplicacao(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<int> CadastrarAsync(Cliente clienteDTO)
        {
            if (clienteDTO == null)
            {
                throw new Exception("Cliente não pode ser vazio");
            }

            return await _clienteRepositorio.CadastrarAsync(clienteDTO);
        }

        public async Task AtualizarAsync(Cliente clienteDTO)
        {
            var clienteExistente = await _clienteRepositorio.ObterPorIdAsync(clienteDTO.Id);

            if (clienteExistente == null)
            {
                throw new Exception("Cliente não encontrado");
            }

            clienteExistente.Nome = clienteDTO.Nome;
            clienteExistente.Endereco = clienteDTO.Endereco;
            clienteExistente.Bairro = clienteDTO.Bairro;
            clienteExistente.Cidade = clienteDTO.Cidade;
            clienteExistente.Telefone = clienteDTO.Telefone;
            clienteExistente.CnpjCpf = clienteDTO.CnpjCpf;

            await _clienteRepositorio.AtualizarAsync(clienteExistente);
        }

        public async Task DeletarAsync(int clienteId)
        {
            var clienteExistente = await _clienteRepositorio.ObterPorIdAsync(clienteId);

            if (clienteExistente == null)
            {
                throw new Exception("Cliente não encontrado");
            }

            await _clienteRepositorio.DeletarAsync(clienteId);
        }

        public async Task RestaurarAsync(int clienteId)
        {
            var clienteExistente = await _clienteRepositorio.ObterPorIdAsync(clienteId);

            if (clienteExistente == null)
            {
                throw new Exception("Cliente não encontrado");
            }

            await _clienteRepositorio.RestaurarAsync(clienteId);
        }

        public async Task<Cliente> ObterPorIdAsync(int clienteId)
        {
            return await _clienteRepositorio.ObterPorIdAsync(clienteId);
        }

        public async Task<IEnumerable<Cliente>> ListarAsync(bool ativo)
        {
            return await _clienteRepositorio.ListarAsync(ativo);
        }
        public async Task<List<Cliente>> ListarTop10ClientesAsync()
{
    return await _clienteRepositorio.ListarTop10ClientesAsync();
}
    }
}