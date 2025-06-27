namespace ChamadosParaCurar.Api.Services
{
    /// <summary>
    /// Classe utilitária para templates de e-mail
    /// </summary>
    public static class EmailTemplates
    {
        /// <summary>
        /// Gera um template de e-mail básico
        /// </summary>
        /// <param name="titulo">Título do e-mail</param>
        /// <param name="conteudo">Conteúdo do e-mail</param>
        /// <param name="botaoTexto">Texto do botão (opcional)</param>
        /// <param name="botaoUrl">URL do botão (opcional)</param>
        /// <returns>HTML formatado do e-mail</returns>
        public static string GerarEmailTemplate(string titulo, string conteudo, string? botaoTexto = null, string? botaoUrl = null)
        {
            var botaoHtml = string.Empty;
            if (!string.IsNullOrEmpty(botaoTexto) && !string.IsNullOrEmpty(botaoUrl))
            {
                botaoHtml = $@"
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin-top: 30px;'>
                    <tr>
                        <td align='center'>
                            <table border='0' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td align='center' bgcolor='#3f51b5' style='border-radius: 6px;'>
                                        <a href='{botaoUrl}' target='_blank' style='padding: 16px 36px; font-family: 'Lato', sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px; display: inline-block; font-weight: bold;'>{botaoTexto}</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>";
            }

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <title>{titulo}</title>
                <style>
                    body, table, td, p, a, li, blockquote {{
                        -webkit-text-size-adjust: 100%;
                        -ms-text-size-adjust: 100%;
                        font-family: 'Lato', 'Helvetica', Arial, sans-serif;
                    }}
                    body {{
                        margin: 0;
                        padding: 0;
                        background-color: #f5f5f5;
                    }}
                </style>
            </head>
            <body>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: #f5f5f5; padding: 20px 0;'>
                    <tr>
                        <td align='center'>
                            <table border='0' cellpadding='0' cellspacing='0' width='600' style='background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                                <!-- Cabeçalho -->
                                <tr>
                                    <td align='center' bgcolor='#3f51b5' style='padding: 30px 0; border-top-left-radius: 8px; border-top-right-radius: 8px;'>
                                        <h1 style='margin: 0; color: #ffffff; font-size: 24px; font-weight: bold;'>Chamados Para Curar</h1>
                                    </td>
                                </tr>
                                <!-- Conteúdo -->
                                <tr>
                                    <td style='padding: 40px 30px;'>
                                        <h2 style='color: #333333; margin-top: 0;'>{titulo}</h2>
                                        <div style='color: #555555; line-height: 1.5; font-size: 16px;'>{conteudo}</div>
                                        {botaoHtml}
                                    </td>
                                </tr>
                                <!-- Rodapé -->
                                <tr>
                                    <td align='center' bgcolor='#f8f9fa' style='padding: 20px 30px; color: #777777; font-size: 14px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px;'>
                                        <p style='margin: 0;'>Chamados Para Curar &copy; {DateTime.Now.Year}</p>
                                        <p style='margin: 10px 0 0 0;'>Esta é uma mensagem automática. Por favor, não responda a este e-mail.</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";
        }

        /// <summary>
        /// Gera um template para e-mail de boas-vindas
        /// </summary>
        /// <param name="nomeUsuario">Nome do usuário</param>
        /// <param name="urlLogin">URL de login</param>
        /// <returns>HTML formatado do e-mail de boas-vindas</returns>
        public static string GerarEmailBoasVindas(string nomeUsuario, string urlLogin)
        {
            var titulo = "Bem-vindo(a) ao Chamados Para Curar";
            var conteudo = $@"
            <p>Olá, <strong>{nomeUsuario}</strong>!</p>
            <p>Estamos muito felizes em tê-lo(a) conosco no Chamados Para Curar.</p>
            <p>Sua conta foi criada com sucesso e você já pode acessar nossos devocionais diários e outros conteúdos exclusivos.</p>
            <p>Esperamos que esta jornada espiritual seja transformadora em sua vida.</p>";

            return GerarEmailTemplate(titulo, conteudo, "Acessar Minha Conta", urlLogin);
        }

        /// <summary>
        /// Gera um template para e-mail de recuperação de senha
        /// </summary>
        /// <param name="nomeUsuario">Nome do usuário</param>
        /// <param name="urlRecuperacao">URL para recuperar a senha</param>
        /// <returns>HTML formatado do e-mail de recuperação de senha</returns>
        public static string GerarEmailRecuperacaoSenha(string nomeUsuario, string urlRecuperacao)
        {
            var titulo = "Recuperação de Senha - Chamados Para Curar";
            var conteudo = $@"
            <p>Olá, <strong>{nomeUsuario}</strong>!</p>
            <p>Recebemos uma solicitação para redefinir a senha da sua conta no Chamados Para Curar.</p>
            <p>Clique no botão abaixo para criar uma nova senha. Este link é válido por 24 horas.</p>
            <p>Se você não solicitou a recuperação de senha, por favor, ignore este e-mail ou entre em contato conosco.</p>";

            return GerarEmailTemplate(titulo, conteudo, "Redefinir Minha Senha", urlRecuperacao);
        }

        /// <summary>
        /// Gera um template para e-mail de novo devocional disponível
        /// </summary>
        /// <param name="nomeUsuario">Nome do usuário</param>
        /// <param name="tituloDevocional">Título do devocional</param>
        /// <param name="urlDevocional">URL para acessar o devocional</param>
        /// <returns>HTML formatado do e-mail de notificação</returns>
        public static string GerarEmailNovoDevocional(string nomeUsuario, string tituloDevocional, string urlDevocional)
        {
            var titulo = "Novo Devocional Disponível";
            var conteudo = $@"
            <p>Olá, <strong>{nomeUsuario}</strong>!</p>
            <p>Temos um novo devocional disponível para você hoje:</p>
            <p style='font-weight: bold; font-size: 18px; margin: 25px 0; padding: 15px; background-color: #f8f9fa; border-left: 4px solid #3f51b5;'>{tituloDevocional}</p>
            <p>Não perca esta oportunidade de reflexão e crescimento espiritual.</p>";

            return GerarEmailTemplate(titulo, conteudo, "Ler o Devocional", urlDevocional);
        }
    }
} 