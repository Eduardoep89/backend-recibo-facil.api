using Microsoft.EntityFrameworkCore;
using ReciboFacil.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Repositorio
{
    public class ItemReciboRepositorio : BaseRepositorio, IItemReciboRepositorio
    {
        public ItemReciboRepositorio(ReciboFacilContexto contexto) : base(contexto)
        {
        }

        // Create: Adicionar um item ao recibo
        public async Task<int> CadastrarAsync(ItemRecibo item)
        {
            await _contexto.ItensRecibo.AddAsync(item);
            await _contexto.SaveChangesAsync();
            return item.Id;
        }

        // Read: Obter um item por ID
        public async Task<ItemRecibo> ObterPorIdAsync(int id)
        {
            return await _contexto.ItensRecibo
                .Include(i => i.Recibo)
                .Include(i => i.Produto)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        // Read: Listar todos os itens de um recibo
        public async Task<List<ItemRecibo>> ListarPorReciboIdAsync(int reciboId)
        {
            return await _contexto.ItensRecibo
                .Include(i => i.Recibo)
                .Include(i => i.Produto)
                .Where(i => i.ReciboId == reciboId)
                .ToListAsync();
        }

        // Update: Atualizar um item do recibo
        public async Task AtualizarAsync(ItemRecibo item)
        {
            _contexto.ItensRecibo.Update(item);
            await _contexto.SaveChangesAsync();
        }

        // Delete: Remover um item do recibo
        public async Task DeletarAsync(int id)
        {
            var item = await _contexto.ItensRecibo.FindAsync(id);

            if (item == null)
            {
                throw new Exception("Item não encontrado.");
            }

            _contexto.ItensRecibo.Remove(item); // Remove o item do banco de dados
            await _contexto.SaveChangesAsync(); // Salva as alterações
        }
    }
}