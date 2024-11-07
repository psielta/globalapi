using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GlobalErpData.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent, string[] imagePaths = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            // Criando a parte HTML do e-mail
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlContent;

            // Adiciona as imagens embutidas
            if (imagePaths != null)
            {
                foreach (var path in imagePaths)
                {
                    var image = builder.LinkedResources.Add(path);
                    image.ContentId = MimeUtils.GenerateMessageId();
                    builder.HtmlBody = builder.HtmlBody.Replace(System.IO.Path.GetFileName(path), $"cid:{image.ContentId}");
                }
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public string LoadTemplate(string templatePath, Dictionary<string, string> placeholders)
        {
            var template = File.ReadAllText(templatePath);

            // Substituir placeholders (ex.: {{Nome}}, {{Link}})
            foreach (var placeholder in placeholders)
            {
                template = Regex.Replace(template, $"{{{{{placeholder.Key}}}}}", placeholder.Value, RegexOptions.IgnoreCase);
            }

            return template;
        }

        public async Task SendTemplatedEmailAsync(string toEmail, string subject, string templatePath, Dictionary<string, string> placeholders, string[] imagePaths = null)
        {
            var htmlContent = LoadTemplate(templatePath, placeholders);
            await SendEmailAsync(toEmail, subject, htmlContent, imagePaths);
        }
    }
}

/*
 
 var placeholders = new Dictionary<string, string>
{
    { "Nome", "Usuário" },
    { "Subject", "Confirmação de Conta" },
    { "Body", "Por favor, clique no link abaixo para confirmar sua conta." },
    { "Link", "https://seusite.com/confirmar-conta" }
};

await emailService.SendTemplatedEmailAsync(
    "usuario@example.com",
    "Confirmação de Conta",
    "path/para/template.html",
    placeholders,
    new[] { "path/para/imagem/logo.png" }
);

 
 */

/*
 
 <!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <title>{{Subject}}</title>
    <style>
        body { font-family: Arial, sans-serif; }
        .header { background-color: #4CAF50; color: white; padding: 10px; }
        .content { padding: 20px; }
        .footer { margin-top: 20px; color: #777; }
    </style>
</head>
<body>
    <div class="header">
        <h1>{{Subject}}</h1>
    </div>
    <div class="content">
        <p>Olá, {{Nome}}</p>
        <p>{{Body}}</p>
        <p><a href="{{Link}}">Clique aqui</a> para confirmar.</p>
    </div>
    <div class="footer">
        <p>&copy; 2024 Sua Empresa - Todos os direitos reservados.</p>
    </div>
</body>
</html>
 */

/*
 var placeholders = new Dictionary<string, string>
{
    { "Nome", "Usuário" },
    { "Subject", "Confirmação de Conta" },
    { "Body", "Por favor, clique no link abaixo para confirmar sua conta." },
    { "Link", "https://seusite.com/confirmar-conta" }
};

// Caminho absoluto para o template (ajuste conforme necessário para seu ambiente)
var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates/Email/EnvioBaseV1.html");

await emailService.SendTemplatedEmailAsync(
    "usuario@example.com",
    "Confirmação de Conta",
    templatePath,
    placeholders,
    new[] { "path/para/imagem/logo.png" }
);
 
 */