// ReciboFacil.Api/Controllers/AutenticacaoController.cs
using Microsoft.AspNetCore.Mvc;
using ReciboFacil.Servicos.Interfaces;
using ReciboFacil.Api.Models;
using System.Threading.Tasks;
using ReciboFacil.Api.Models.LoginModel;
using ReciboFacil.Api.Models.RegistroModel;

namespace ReciboFacil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoServico _autenticacaoServico;

        public AutenticacaoController(IAutenticacaoServico autenticacaoServico)
        {
            _autenticacaoServico = autenticacaoServico;
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroModel model)
        {
            try
            {
                var userId = await _autenticacaoServico.RegistrarAsync(
                    model.Nome,
                    model.Email,
                    model.Senha);

                return Ok(new { Id = userId, Message = "Registro realizado com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var autenticado = await _autenticacaoServico.AutenticarAsync(model.Email, model.Senha);
            if (!autenticado)
            {
                return Unauthorized("Credenciais inválidas");
            }

            return Ok(new { message = "Login realizado com sucesso" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoServico.EncerrarSessaoAsync();
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        [HttpGet("verificar-sessao")]
        public async Task<IActionResult> VerificarSessao()
        {
            var sessaoAtiva = await _autenticacaoServico.ValidarSessaoAsync();
            return Ok(new { sessaoAtiva });
        }
    }
}