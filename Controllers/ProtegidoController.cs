using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChamadosParaCurar.Api.Controllers
{
    [ApiController]
    [Route("api/protegido")]
    public class ProtegidoController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("Endpoint protegido acessível para qualquer usuário autenticado!");
        }

        [HttpGet("pago")]
        [Authorize(Roles = "Pago")]
        public IActionResult GetPago()
        {
            return Ok("Endpoint protegido acessível apenas para usuários pagos!");
        }

        [HttpGet("perfil")]
        [Authorize]
        public IActionResult GetPerfil()
        {
            // Acessando as claims do usuário autenticado
            var usuarioId = User.Identity?.Name;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var tipo = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new { 
                mensagem = "Dados do perfil do usuário autenticado",
                nome = usuarioId,
                email,
                tipo
            });
        }

        [HttpGet("teste-api")]
        [AllowAnonymous]
        public IActionResult TesteApi()
        {
            return Ok(new {
                mensagem = "API está funcionando corretamente!",
                timestamp = DateTime.UtcNow
            });
        }
    }
} 