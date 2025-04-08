using ReciboFacil.Aplicacao.Interfaces;
using ReciboFacil.Servicos.Interfaces;
using Microsoft.AspNetCore.Http;
using BCrypt.Net; // Adicione este using para o hash de senha
using System;
using ReciboFacil.Dominio.Entidades; // Para InvalidOperationException

namespace ReciboFacil.Servicos.Implementacoes
{
    public class AutenticacaoServico : IAutenticacaoServico
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IHttpContextAccessor _contextoAcesso;

        public AutenticacaoServico(
            IUsuarioAplicacao usuarioAplicacao,
            IHttpContextAccessor contextoAcesso)
        {
            _usuarioAplicacao = usuarioAplicacao;
            _contextoAcesso = contextoAcesso;
        }

        // Método NOVO para registro
        public async Task<int> RegistrarAsync(string nome, string email, string senha)
        {
            // Verifica se usuário já existe
            var usuarioExistente = await _usuarioAplicacao.ObterPorEmailAsync(email);
            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("Email já está em uso");
            }

            // Cria novo usuário com senha hasheada
            var novoUsuario = new Usuario
            {
                Nome = nome,
                Email = email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };

            // Salva no banco e retorna o ID
            return await _usuarioAplicacao.CriarAsync(novoUsuario);
        }

        // Métodos EXISTENTES (mantidos exatamente como estão)
        public async Task<bool> AutenticarAsync(string email, string senha)
        {
            var usuario = await _usuarioAplicacao.AutenticarAsync(email, senha);
            if (usuario == null) return false;

            await RegistrarSessaoAsync(email);
            return true;
        }

        public async Task RegistrarSessaoAsync(string email)
        {
            _contextoAcesso.HttpContext.Session.SetString("UsuarioEmail", email);
            await Task.CompletedTask;
        }

        public async Task<bool> ValidarSessaoAsync()
        {
            var email = _contextoAcesso.HttpContext.Session.GetString("UsuarioEmail");
            await Task.CompletedTask;
            return !string.IsNullOrEmpty(email);
        }

        public async Task EncerrarSessaoAsync()
        {
            _contextoAcesso.HttpContext.Session.Remove("UsuarioEmail");
            await Task.CompletedTask;
        }
    }
}