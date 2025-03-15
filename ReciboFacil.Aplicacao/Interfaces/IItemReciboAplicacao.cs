using ReciboFacil.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public interface IItemReciboAplicacao
    {
        Task<int> CadastrarAsync(ItemRecibo itemReciboDTO);
        Task AtualizarAsync(ItemRecibo itemReciboDTO);
        Task DeletarAsync(int itemReciboId);
        Task<ItemRecibo> ObterPorIdAsync(int itemReciboId);
        Task<IEnumerable<ItemRecibo>> ListarPorReciboIdAsync(int reciboId);
    }
}