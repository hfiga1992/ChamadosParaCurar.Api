using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Controllers
{
    [ApiController]
    [Route("api/certificados")]
    public class CertificadosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly CertificadoService _certificadoService;

        public CertificadosController(IUsuarioService usuarioService, CertificadoService certificadoService)
        {
            _usuarioService = usuarioService;
            _certificadoService = certificadoService;
        }

        [HttpGet("{usuarioId}")]
        [Authorize(Roles = "Pago")]
        public async Task<IActionResult> GerarCertificado(string usuarioId)
        {
            // Verificar se o ID é válido
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("ID de usuário inválido");
            }

            // Buscar o usuário pelo ID
            var usuario = await _usuarioService.ObterPorId(usuarioId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Verificar se o usuário atual está tentando acessar seu próprio certificado
            // ou se é um admin (esta validação seria opcional)
            
            try
            {
                // Gerar o certificado
                var pdfBytes = _certificadoService.GerarCertificado(usuario);
                
                // Retornar o PDF como arquivo para download
                return File(
                    fileContents: pdfBytes,
                    contentType: "application/pdf",
                    fileDownloadName: $"certificado-{usuario.Nome.ToLower().Replace(" ", "-")}.pdf"
                );
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar certificado: {ex.Message}");
            }
        }
    }
} 