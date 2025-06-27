using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChamadosParaCurar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Envia um e-mail de teste para o endereço especificado
        /// </summary>
        /// <param name="request">Objeto com o e-mail do destinatário</param>
        /// <returns>Resultado da operação de envio</returns>
        [HttpPost("teste")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> TestarEnvioEmail([FromBody] EmailTesteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { erro = "O e-mail do destinatário é obrigatório" });
            }

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                string urlAplicacao = $"{baseUrl}";
                
                // Cria um conteúdo HTML personalizado para o e-mail de teste
                string titulo = "Teste de E-mail - Chamados Para Curar";
                string conteudo = @"
                    <p>Este é um e-mail de teste enviado pelo sistema <strong>Chamados Para Curar</strong>.</p>
                    <p>Se você está recebendo este e-mail, a configuração do serviço de e-mail foi realizada com sucesso.</p>
                    <p>Dados técnicos:</p>
                    <ul>
                        <li>Data e hora do envio: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</li>
                        <li>Servidor: " + Environment.MachineName + @"</li>
                    </ul>
                ";
                
                string corpoHtml = EmailTemplates.GerarEmailTemplate(
                    titulo,
                    conteudo,
                    "Acessar Aplicação",
                    urlAplicacao);
                
                await _emailService.EnviarEmailAsync(
                    request.Email,
                    titulo,
                    corpoHtml);

                return Ok(new { 
                    sucesso = true, 
                    mensagem = "E-mail enviado com sucesso",
                    detalhes = new { 
                        destinatario = request.Email,
                        dataEnvio = DateTime.Now
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    sucesso = false, 
                    erro = "Erro ao enviar e-mail", 
                    detalhes = ex.Message 
                });
            }
        }
    }

    /// <summary>
    /// Modelo de requisição para o teste de envio de e-mail
    /// </summary>
    public class EmailTesteRequest
    {
        /// <summary>
        /// E-mail do destinatário
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
} 