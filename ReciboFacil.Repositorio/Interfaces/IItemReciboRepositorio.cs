using ReciboFacil.Dominio.Entidades;

namespace ReciboFacil.Repositorio
{
    public interface IItemReciboRepositorio
    {
        Task<int> CadastrarAsync(ItemRecibo item);
        Task<ItemRecibo> ObterPorIdAsync(int id);
        Task<List<ItemRecibo>> ListarPorReciboIdAsync(int reciboId);
        Task AtualizarAsync(ItemRecibo item);
        Task DeletarAsync(int id);
    }
}