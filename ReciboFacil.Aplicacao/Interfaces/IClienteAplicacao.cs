using ReciboFacil.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public interface IClienteAplicacao
    {
        Task<int> CadastrarAsync(Cliente clienteDTO);
        Task AtualizarAsync(Cliente clienteDTO);
        Task DeletarAsync(int clienteId);
        Task RestaurarAsync(int clienteId);
        Task<Cliente> ObterPorIdAsync(int clienteId);
        Task<IEnumerable<Cliente>> ListarAsync(bool ativo);
         Task<List<Cliente>> ListarTop10ClientesAsync();
    }
}