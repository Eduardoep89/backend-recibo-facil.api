using ReciboFacil.Dominio.Entidades;

namespace ReciboFacil.Repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> ObterPorEmailAsync(string email);
        Task<int> CriarAsync(Usuario usuario);
    }
}