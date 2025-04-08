// ReciboFacil.Aplicacao/Interfaces/IUsuarioAplicacao.cs
using ReciboFacil.Dominio.Entidades;

namespace ReciboFacil.Aplicacao.Interfaces
{
    public interface IUsuarioAplicacao
    {
        Task<Usuario> ObterPorEmailAsync(string email); // Adicione este método
        Task<Usuario> AutenticarAsync(string email, string senha);
        Task<int> CriarAsync(Usuario usuario);
    }
}