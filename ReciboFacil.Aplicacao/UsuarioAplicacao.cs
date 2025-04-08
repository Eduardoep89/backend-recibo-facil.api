// ReciboFacil.Aplicacao/Aplicacoes/UsuarioAplicacao.cs
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio.Interfaces;
using System.Threading.Tasks;
using BCrypt.Net;
using ReciboFacil.Aplicacao.Interfaces;

namespace ReciboFacil.Aplicacao.Aplicacoes
{
    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            return await _usuarioRepositorio.ObterPorEmailAsync(email);
        }

        public async Task<int> CriarAsync(Usuario usuario)
        {

            return await _usuarioRepositorio.CriarAsync(usuario);
        }

        public async Task<Usuario> AutenticarAsync(string email, string senha)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmailAsync(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            {
                return null;
            }
            return usuario;
        }
    }
}