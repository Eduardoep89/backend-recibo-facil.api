using Microsoft.EntityFrameworkCore;
using ReciboFacil.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciboFacil.Repositorio
{
    public class ReciboRepositorio : BaseRepositorio, IReciboRepositorio
    {
        public ReciboRepositorio(ReciboFacilContexto contexto) : base(contexto)
        {
        }

        // Create: Cadastrar um novo recibo
        public async Task<int> CadastrarAsync(Recibo recibo)
        {
            await _contexto.Recibos.AddAsync(recibo);
            await _contexto.SaveChangesAsync();
            return recibo.Id;
        }

        // Read: Obter um recibo por ID
        public async Task<Recibo> ObterPorIdAsync(int id)
        {
            return await _contexto.Recibos
                .Include(r => r.Cliente)
                .Include(r => r.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // Read: Listar todos os recibos (com filtros opcionais)
       public async Task<List<Recibo>> ListarAsync(bool ativo = true)
{
    return await _contexto.Recibos
        .Include(r => r.Cliente)
        .Include(r => r.Itens)
        .ThenInclude(i => i.Produto)
        .Where(r => r.Ativo == ativo) // Filtra por recibos ativos ou inativos
        .ToListAsync();
}

        // Update: Atualizar um recibo existente
        public async Task AtualizarAsync(Recibo recibo)
        {
            _contexto.Recibos.Update(recibo);
            await _contexto.SaveChangesAsync();
        }

        // Delete: Excluir um recibo (soft delete)
        public async Task DeletarAsync(int id)
        {
            var recibo = await _contexto.Recibos.FindAsync(id);

            if (recibo == null)
            {
                throw new Exception("Recibo não encontrado.");
            }

            recibo.Deletar(); // Marca o recibo como inativo (soft delete)
            _contexto.Recibos.Update(recibo); // Atualiza o recibo no banco de dados
            await _contexto.SaveChangesAsync(); // Salva as alterações
        }

        // Read: Listar recibos por cliente
        public async Task<List<Recibo>> ListarPorClienteIdAsync(int clienteId)
        {
            return await _contexto.Recibos
                .Include(r => r.Cliente)
                .Include(r => r.Itens)
                .ThenInclude(i => i.Produto)
                .Where(r => r.ClienteId == clienteId && r.Ativo)
                .ToListAsync();
        }
        public async Task<List<Recibo>> ListarTop10RecibosAsync()
{
    return await _contexto.Recibos
        .Include(r => r.Cliente)
        .Include(r => r.Itens)
        .ThenInclude(i => i.Produto)
        .OrderByDescending(r => r.Data) // Ordena pela data mais recente
        .Take(10) // Limita a 10 recibos
        .ToListAsync();
}
    }
}