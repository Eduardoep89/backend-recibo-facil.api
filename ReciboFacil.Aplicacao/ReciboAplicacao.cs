using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciboFacil.Aplicacao
{
    public class ReciboAplicacao : IReciboAplicacao
    {
        private readonly IReciboRepositorio _reciboRepositorio;

        public ReciboAplicacao(IReciboRepositorio reciboRepositorio)
        {
            _reciboRepositorio = reciboRepositorio;
        }

        // Cadastrar um novo recibo
        public async Task<int> CadastrarAsync(Recibo reciboDTO)
        {
            if (reciboDTO == null)
            {
                throw new ArgumentNullException(nameof(reciboDTO), "Recibo não pode ser nulo.");
            }

            return await _reciboRepositorio.CadastrarAsync(reciboDTO);
        }

        // Atualizar um recibo existente
        public async Task AtualizarAsync(Recibo reciboDTO)
        {
            if (reciboDTO == null)
            {
                throw new ArgumentNullException(nameof(reciboDTO), "Recibo não pode ser nulo.");
            }

            await _reciboRepositorio.AtualizarAsync(reciboDTO);
        }

        // Deletar um recibo (soft delete)
        public async Task DeletarAsync(int reciboId)
        {
            await _reciboRepositorio.DeletarAsync(reciboId);
        }

        // Restaurar um recibo deletado
        public async Task RestaurarAsync(int reciboId)
        {
            var recibo = await _reciboRepositorio.ObterPorIdAsync(reciboId);

            if (recibo == null)
            {
                throw new Exception("Recibo não encontrado.");
            }

            recibo.Restaurar();
            await _reciboRepositorio.AtualizarAsync(recibo);
        }

        // Obter um recibo por ID
        public async Task<Recibo> ObterPorIdAsync(int reciboId)
        {
            return await _reciboRepositorio.ObterPorIdAsync(reciboId);
        }

        // Listar todos os recibos (com filtro de ativo/inativo)
        public async Task<IEnumerable<Recibo>> ListarAsync(bool ativo)
        {
            return await _reciboRepositorio.ListarAsync(ativo);
        }

        // Listar recibos por cliente
        public async Task<List<Recibo>> ListarPorClienteIdAsync(int clienteId)
        {
            return await _reciboRepositorio.ListarPorClienteIdAsync(clienteId);
        }

        // Listar os 10 últimos recibos
        public async Task<List<Recibo>> ListarTop10RecibosAsync()
        {
            return await _reciboRepositorio.ListarTop10RecibosAsync();
        }
    }
}