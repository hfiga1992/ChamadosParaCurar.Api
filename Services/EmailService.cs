using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly SendGridSettings _settings;

        public EmailService(IOptions<SendGridSettings> options)
        {
            _settings = options.Value;
            _client = new SendGridClient(_settings.ApiKey);
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml)
        {
            await EnviarEmailAsync(destinatario, assunto, corpoHtml, _settings.RemetenteNome);
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml, string nomeRemetente)
        {
            if (string.IsNullOrWhiteSpace(destinatario))
                throw new ArgumentException("O endereço de e-mail do destinatário não pode estar vazio", nameof(destinatario));

            if (string.IsNullOrWhiteSpace(assunto))
                throw new ArgumentException("O assunto do e-mail não pode estar vazio", nameof(assunto));

            if (string.IsNullOrWhiteSpace(corpoHtml))
                throw new ArgumentException("O corpo do e-mail não pode estar vazio", nameof(corpoHtml));

            var from = new EmailAddress(_settings.RemetenteEmail, nomeRemetente);
            var to = new EmailAddress(destinatario);
            var msg = MailHelper.CreateSingleEmail(from, to, assunto, null, corpoHtml);
            
            var response = await _client.SendEmailAsync(msg);
            
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"Falha ao enviar e-mail. Status: {response.StatusCode}, Detalhes: {responseBody}");
            }
        }
    }
} 