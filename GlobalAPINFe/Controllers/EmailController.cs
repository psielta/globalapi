using GlobalErpData.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly EmailService emailService;
    public EmailController(EmailService emailService)
    {
        this.emailService = emailService;
    }

    [HttpGet("/{email}")]
    public async Task<IActionResult> Get(string email)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "Nome", "Usuário" },
            { "Subject", "Confirmação de Conta" },
            { "Body", "Por favor, clique no link abaixo para confirmar sua conta." },
            { "Link", "https://seusite.com/confirmar-conta" }
        };

        var templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates/Email/EnvioBaseV1.html");
        var logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Global/logo.jpeg");

        await emailService.SendTemplatedEmailAsync(
            email,
            "Confirmação de Conta",
            templatePath,
            placeholders,
            new[] { logoPath }
        );

        return Ok("Email enviado com sucesso!");
    }
}
