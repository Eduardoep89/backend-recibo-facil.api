using Microsoft.AspNetCore.Mvc;
using ReciboFacil.Servicos.Interfaces;
using System;
using System.Threading.Tasks;

namespace ReciboFacil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly ILogger<RelatoriosController> _logger;

        public RelatoriosController(IAIService aiService, ILogger<RelatoriosController> logger)
        {
            _aiService = aiService;
            _logger = logger;
        }

        [HttpGet("analitico")]
        public async Task<IActionResult> GerarRelatorioAnalitico()
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var relatorio = await _aiService.GerarRelatorioAnaliticoAsync();
                stopwatch.Stop();

                _logger.LogInformation($"Relatório gerado em {stopwatch.ElapsedMilliseconds}ms");
                return Ok(new { relatorio, tempoMs = stopwatch.ElapsedMilliseconds });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha no endpoint analitico");
                return StatusCode(500, new { erro = "Falha ao gerar relatório", detalhes = ex.Message });
            }
        }

        [HttpGet("sugestoes/{clienteId}")]
        public async Task<IActionResult> GerarSugestoesProdutos(int clienteId)
        {
            try
            {
                var sugestoes = await _aiService.GerarSugestoesProdutosAsync(clienteId);
                return Ok(new { sugestoes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Falha ao gerar sugestões para cliente {clienteId}");
                return StatusCode(500, new { erro = "Falha ao gerar sugestões" });
            }
        }

        [HttpPost("personalizado")]
        public async Task<IActionResult> GerarRelatorioPersonalizado([FromBody] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest(new { erro = "Prompt não pode ser vazio" });

            try
            {
                var resultado = await _aiService.GerarRelatorioPersonalizadoAsync(prompt);
                return Ok(new { resultado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Falha no prompt: {prompt}");
                return StatusCode(500, new { erro = "Falha ao processar prompt" });
            }
        }
    }
}