using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Models;
using ChamadosParaCurar.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UsuariosController(IUsuarioService usuarioService, IEmailService emailService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet("me")]
        public async Task<ActionResult<Usuario>> GetMeuPerfil()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado");
            }

            var usuario = await _usuarioService.ObterPorId(userId);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Remover a senha antes de retornar
            usuario.Senha = null!;
            return Ok(usuario);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(string id)
        {
            // Verificar se o usuário atual tem permissão para acessar este recurso
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != id)
            {
                return Forbid("Você não tem permissão para acessar este recurso");
            }

            var usuario = await _usuarioService.ObterPorId(id);
            
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Não retornar a senha
            usuario.Senha = null!;

            return Ok(usuario);
        }

        // Exemplo de uso do serviço de e-mail para enviar um e-mail de teste
        [HttpPost("teste-email")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem testar o envio de e-mail
        public async Task<IActionResult> TesteEmail([FromBody] TesteEmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Destinatario))
            {
                return BadRequest("O e-mail do destinatário é obrigatório");
            }

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                string urlDevocional = $"{baseUrl}/devocionais";
                
                // Usando o template de e-mail de boas-vindas
                string corpoHtml = EmailTemplates.GerarEmailBoasVindas(
                    request.Nome ?? "Usuário",
                    urlDevocional);
                    
                await _emailService.EnviarEmailAsync(
                    request.Destinatario,
                    "Teste de E-mail - Chamados Para Curar",
                    corpoHtml);

                return Ok(new { 
                    mensagem = "E-mail enviado com sucesso",
                    destinatario = request.Destinatario
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao enviar e-mail: {ex.Message}" });
            }
        }
    }

    public class TesteEmailRequest
    {
        public string Destinatario { get; set; } = string.Empty;
        public string? Nome { get; set; }
    }
} 