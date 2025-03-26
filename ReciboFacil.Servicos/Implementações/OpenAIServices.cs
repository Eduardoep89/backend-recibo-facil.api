using System;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using ReciboFacil.Aplicacao;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Servicos.Interfaces;

namespace ReciboFacil.Servicos.Implementacoes
{
    public class OpenAIService : IAIService
    {
        private readonly OpenAIClient _client;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IProdutoAplicacao _produtoAplicacao;
        private readonly IReciboAplicacao _reciboAplicacao;
        private readonly IMemoryCache _cache;
        private readonly ILogger<OpenAIService> _logger;
        private readonly string _modelName;
        private readonly float _temperature;
        private readonly int _maxTokens;
        private readonly TimeSpan _cacheDuration;

        public OpenAIService(
            OpenAIClient client,
            IConfiguration configuration,
            IClienteAplicacao clienteAplicacao,
            IProdutoAplicacao produtoAplicacao,
            IReciboAplicacao reciboAplicacao,
            IMemoryCache cache,
            ILogger<OpenAIService> logger)
        {
            _client = client;
            _clienteAplicacao = clienteAplicacao;
            _produtoAplicacao = produtoAplicacao;
            _reciboAplicacao = reciboAplicacao;
            _cache = cache;
            _logger = logger;

            // Configurações com validações
            _modelName = configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";

            if (!float.TryParse(configuration["OpenAI:Temperature"], out _temperature) ||
                _temperature < 0 || _temperature > 2)
            {
                _temperature = 0.7f;
                _logger.LogWarning("Temperature inválida. Usando valor padrão 0.7");
            }

            if (!int.TryParse(configuration["OpenAI:MaxTokens"], out _maxTokens) ||
                _maxTokens <= 0)
            {
                _maxTokens = 500;
                _logger.LogWarning("MaxTokens inválido. Usando valor padrão 500");
            }

            if (!int.TryParse(configuration["CacheSettings:DefaultCacheDurationMinutes"],
                out var cacheMinutes) || cacheMinutes <= 0)
            {
                cacheMinutes = 30;
                _logger.LogWarning("CacheDuration inválido. Usando valor padrão 30 minutos");
            }
            _cacheDuration = TimeSpan.FromMinutes(cacheMinutes);
        }

        public async Task<string> GerarRelatorioAnaliticoAsync()
        {
            var cacheKey = $"relatorio_analitico_{DateTime.UtcNow:yyyyMMdd}";

            if (_cache.TryGetValue(cacheKey, out string cachedReport))
                return cachedReport;

            try
            {
                var recibos = await _reciboAplicacao.ListarAsync(true);
                var prompt = $"Gere um relatório analítico conciso com os seguintes dados:\n" +
                             $"1. Total de recibos: {recibos.Count()}\n" +
                             $"2. Valor médio: {recibos.Average(r => r.Total):C}\n" +
                             $"3. Top 3 clientes por volume\n" +
                             $"4. Principais insights em bullet points";

                var relatorio = await ChamarOpenAIAsync(prompt);
                _cache.Set(cacheKey, relatorio, _cacheDuration);

                return relatorio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao gerar relatório analítico");
                return $"Erro ao gerar relatório: {ex.Message}";
            }
        }

        public async Task<string> GerarSugestoesProdutosAsync(int clienteId)
        {
            var cacheKey = $"sugestoes_{clienteId}_{DateTime.UtcNow:yyyyMMdd}";

            if (_cache.TryGetValue(cacheKey, out string cachedSuggestions))
                return cachedSuggestions;

            try
            {
                var cliente = await _clienteAplicacao.ObterPorIdAsync(clienteId);
                var produtos = await _produtoAplicacao.ListarProdutosPorClienteIdAsync(clienteId);

                var prompt = $"Baseado no histórico, sugira 3 produtos para {cliente.Nome} que já possui:\n" +
                             string.Join("\n", produtos.Select(p => $"- {p.Nome} ({p.Marca})")) +
                             "\n\nFormato esperado:\n1. [Produto] - [Razão]\n2. [Produto] - [Razão]\n3. [Produto] - [Razão]";

                var sugestoes = await ChamarOpenAIAsync(prompt);
                _cache.Set(cacheKey, sugestoes, TimeSpan.FromMinutes(20));

                return sugestoes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao gerar sugestões para cliente {ClienteId}", clienteId);
                return $"Erro ao gerar sugestões: {ex.Message}";
            }
        }

        public async Task<string> GerarRelatorioPersonalizadoAsync(string promptPersonalizado)
        {
            if (string.IsNullOrWhiteSpace(promptPersonalizado))
            {
                throw new ArgumentException("Prompt não pode ser vazio", nameof(promptPersonalizado));
            }

            return await ChamarOpenAIAsync($"Responda de forma detalhada e estruturada: {promptPersonalizado}");
        }

        private async Task<string> ChamarOpenAIAsync(string prompt)
        {
            try
            {
                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    Messages =
                    {
                        new ChatRequestSystemMessage("Você é um assistente especializado em análise de dados comerciais."),
                        new ChatRequestUserMessage(prompt)
                    },
                    MaxTokens = _maxTokens,
                    Temperature = _temperature,
                    DeploymentName = _modelName
                };

                var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

                if (response?.Value?.Choices?.Count == 0)
                {
                    throw new InvalidOperationException("Nenhuma resposta gerada pela API");
                }

                return response.Value.Choices[0].Message.Content;
            }
            catch (RequestFailedException ex) when (ex.Status == 401)
            {
                _logger.LogError("Autenticação falhou - verifique a chave da API");
                throw new UnauthorizedAccessException("Falha de autenticação com a OpenAI");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na chamada à OpenAI. Prompt: {Prompt}", prompt);
                throw;
            }
        }
    }
}