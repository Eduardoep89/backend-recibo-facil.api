using ReciboFacil.Dominio.Entidades;

    public interface IReciboRepositorio
    {
        Task<int> CadastrarAsync(Recibo recibo);
        Task<Recibo> ObterPorIdAsync(int id);
        Task<List<Recibo>> ListarAsync(bool ativo = true);
        Task AtualizarAsync(Recibo recibo);
        Task DeletarAsync(int id);
        Task<List<Recibo>> ListarPorClienteIdAsync(int clienteId);
        Task<List<Recibo>> ListarTop10RecibosAsync();
    }
