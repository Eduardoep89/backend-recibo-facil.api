// ReciboFacil.Repositorio/Repositorios/UsuarioRepositorio.cs
using Dapper;
using Microsoft.Data.SqlClient;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio.Interfaces;
using System.Data;

namespace ReciboFacil.Repositorio.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IDbConnection _contexto;

        public UsuarioRepositorio(IDbConnection contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            using var conexao = new SqlConnection(_contexto.ConnectionString);

            var query = @"
                SELECT Id, Nome, Email, SenhaHash, DataCriacao, Ativo 
                FROM Usuarios 
                WHERE Email = @Email";

            return await conexao.QueryFirstOrDefaultAsync<Usuario>(query, new { Email = email });
        }

        public async Task<int> CriarAsync(Usuario usuario)
        {
            using var conexao = new SqlConnection(_contexto.ConnectionString);

            var query = @"
                INSERT INTO Usuarios (Nome, Email, SenhaHash, Ativo, DataCriacao)
                OUTPUT INSERTED.Id
                VALUES (@Nome, @Email, @SenhaHash, @Ativo, @DataCriacao)";

            return await conexao.ExecuteScalarAsync<int>(query, usuario);
        }
    }
}