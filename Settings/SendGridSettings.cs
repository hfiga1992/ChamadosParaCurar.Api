namespace ChamadosParaCurar.Api.Settings
{
    public class SendGridSettings
    {
        /// <summary>
        /// Chave de API do SendGrid
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;
        
        /// <summary>
        /// Email do remetente padrão
        /// </summary>
        public string RemetenteEmail { get; set; } = string.Empty;
        
        /// <summary>
        /// Nome do remetente padrão
        /// </summary>
        public string RemetenteNome { get; set; } = string.Empty;
    }
} 