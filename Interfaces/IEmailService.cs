using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Envia um e-mail para o destinatário especificado
        /// </summary>
        /// <param name="destinatario">Endereço de e-mail do destinatário</param>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="corpoHtml">Corpo do e-mail em formato HTML</param>
        /// <returns>Tarefa assíncrona representando o envio do e-mail</returns>
        Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml);
        
        /// <summary>
        /// Envia um e-mail com remetente personalizado
        /// </summary>
        /// <param name="destinatario">Endereço de e-mail do destinatário</param>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="corpoHtml">Corpo do e-mail em formato HTML</param>
        /// <param name="nomeRemetente">Nome personalizado do remetente</param>
        /// <returns>Tarefa assíncrona representando o envio do e-mail</returns>
        Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml, string nomeRemetente);
    }
} 