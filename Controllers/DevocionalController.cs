using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de devocionais
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class DevocionaisController : ControllerBase
    {
        private readonly IDevocionalService _devocionalService;
        private readonly IUsuarioService _usuarioService;

        public DevocionaisController(IDevocionalService devocionalService, IUsuarioService usuarioService)
        {
            _devocionalService = devocionalService;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Obtém o devocional do dia atual
        /// </summary>
        /// <returns>O devocional do dia atual</returns>
        /// <response code="200">Retorna o devocional do dia</response>
        /// <response code="404">Não há devocional disponível para hoje</response>
        [HttpGet("hoje")]
        [ProducesResponseType(typeof(Devocional), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Devocional>> ObterDevocionalHoje()
        {
            var devocional = await _devocionalService.ObterDevocionalHoje();

            if (devocional == null)
            {
                return NotFound("Não há devocional disponível para hoje.");
            }

            // Verificar se o usuário já leu o devocional de hoje
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var dataHoje = DateOnly.FromDateTime(DateTime.Today);
                var jaLido = await _usuarioService.VerificarDevocionalLido(userId, dataHoje);
                
                // Adiciona informação se o devocional já foi lido pelo usuário
                HttpContext.Response.Headers.Append("X-Devocional-Lido", jaLido.ToString().ToLower());
            }

            return Ok(devocional);
        }

        /// <summary>
        /// Lista todos os devocionais cadastrados
        /// </summary>
        /// <returns>Lista de devocionais cadastrados</returns>
        /// <response code="200">Retorna a lista de devocionais</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Devocional>), 200)]
        public async Task<ActionResult<IEnumerable<Devocional>>> ListarTodos()
        {
            var devocionais = await _devocionalService.ListarTodos();
            return Ok(devocionais);
        }
        
        /// <summary>
        /// Marca o devocional do dia como lido pelo usuário autenticado
        /// </summary>
        /// <returns>Status da operação</returns>
        /// <response code="200">Devocional marcado como lido com sucesso</response>
        /// <response code="401">Usuário não autenticado</response>
        /// <response code="404">Usuário não encontrado ou devocional não disponível para hoje</response>
        [HttpPost("ler")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> MarcarComoLido()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado ou token inválido");
            }

            var devocional = await _devocionalService.ObterDevocionalHoje();
            if (devocional == null)
            {
                return NotFound("Não há devocional disponível para hoje.");
            }

            var dataHoje = DateOnly.FromDateTime(DateTime.Today);
            var sucesso = await _usuarioService.MarcarDevocionalComoLido(userId, dataHoje);

            if (!sucesso)
            {
                return NotFound("Usuário não encontrado");
            }

            return Ok(new { sucesso = true, mensagem = "Devocional marcado como lido com sucesso" });
        }

        /// <summary>
        /// Cadastra um novo devocional
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/devocionais
        ///     {
        ///        "titulo": "Confie no Senhor",
        ///        "conteudo": "Mesmo em meio às tempestades, Deus está com você. Ele é seu refúgio e fortaleza.",
        ///        "data": "2025-06-21"
        ///     }
        ///
        /// </remarks>
        /// <param name="devocional">Dados do devocional a ser cadastrado</param>
        /// <returns>O devocional cadastrado</returns>
        /// <response code="201">Devocional cadastrado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="409">Já existe um devocional para a data informada</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Devocional), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<Devocional>> CriarDevocional([FromBody] Devocional devocional)
        {
            if (devocional == null)
            {
                return BadRequest("Dados do devocional não podem ser nulos.");
            }

            if (string.IsNullOrWhiteSpace(devocional.Titulo))
            {
                return BadRequest("O título do devocional é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(devocional.Conteudo))
            {
                return BadRequest("O conteúdo do devocional é obrigatório.");
            }

            if (devocional.Data == default)
            {
                return BadRequest("A data do devocional é obrigatória e deve estar no formato ISO 8601 (ex: 2025-06-21).");
            }

            try
            {
                var novoDevocional = await _devocionalService.CriarDevocional(devocional);
                return CreatedAtAction(nameof(ObterDevocionalHoje), novoDevocional);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar devocional: {ex.Message}");
            }
        }
    }
} 