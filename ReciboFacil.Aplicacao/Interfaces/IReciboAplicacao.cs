using ReciboFacil.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public interface IReciboAplicacao
    {
        Task<int> CadastrarAsync(Recibo reciboDTO);
        Task AtualizarAsync(Recibo reciboDTO);
        Task DeletarAsync(int reciboId);
        Task RestaurarAsync(int reciboId);
        Task<Recibo> ObterPorIdAsync(int reciboId);
        Task<IEnumerable<Recibo>> ListarAsync(bool ativo);
        Task<List<Recibo>> ListarPorClienteIdAsync(int clienteId);
        Task<List<Recibo>> ListarTop10RecibosAsync();
    }
}