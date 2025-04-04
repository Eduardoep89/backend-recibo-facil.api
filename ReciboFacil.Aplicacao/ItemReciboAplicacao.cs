using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public class ItemReciboAplicacao : IItemReciboAplicacao
    {
        private readonly IItemReciboRepositorio _itemRepositorio;

        public ItemReciboAplicacao(IItemReciboRepositorio itemRepositorio)
        {
            _itemRepositorio = itemRepositorio;
        }

        public async Task<int> CadastrarAsync(ItemRecibo itemReciboDTO)
        {
            if (itemReciboDTO == null)
            {
                throw new ArgumentNullException(nameof(itemReciboDTO), "Item de recibo não pode ser nulo.");
            }

            return await _itemRepositorio.CadastrarAsync(itemReciboDTO);
        }

        public async Task AtualizarAsync(ItemRecibo itemReciboDTO)
        {
            if (itemReciboDTO == null)
            {
                throw new ArgumentNullException(nameof(itemReciboDTO), "Item de recibo não pode ser nulo.");
            }

            await _itemRepositorio.AtualizarAsync(itemReciboDTO);
        }

        public async Task DeletarAsync(int itemReciboId)
        {
            await _itemRepositorio.DeletarAsync(itemReciboId);
        }

        public async Task<ItemRecibo> ObterPorIdAsync(int itemReciboId)
        {
            return await _itemRepositorio.ObterPorIdAsync(itemReciboId);
        }

        // Listar itens de recibo por reciboId
        public async Task<IEnumerable<ItemRecibo>> ListarPorReciboIdAsync(int reciboId)
        {
            return await _itemRepositorio.ListarPorReciboIdAsync(reciboId);
        }
    }
}