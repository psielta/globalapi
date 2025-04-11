using Serilog;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

// Configurar o Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Services.AddDbContext<GlobalErpData.Data.GlobalErpFiscalBaseContext>(options =>
        options.UseNpgsql(IniFile.GetConnectionString())); 

    var chaveSeguranca = builder.Configuration["CHAVE_SEGURANCA"];

    var app = builder.Build();

    app.Use(async (context, next) =>
    {
        Log.Information("Recebendo requisição: {Method} {Path}", context.Request.Method, context.Request.Path);
        await next();
    });

    app.MapPost("/uairangoWebhook", async (HttpContext context, GlobalErpData.Data.GlobalErpFiscalBaseContext dbContext) => // Injeção do DbContext aqui
    {
        var xUairangoKey = context.Request.Headers["x-uairango-key"].ToString();

        if (!xUairangoKey.Equals(chaveSeguranca))
        {
            Log.Warning("Tentativa de acesso com chave inválida");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Chave inválida!");
            return;
        }

        Log.Information("Autenticação bem-sucedida para webhook");
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();

        Log.Information("Corpo da requisição: {Body}", body);

        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync("OK");
    });

    // Inicia a aplicação
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação terminou inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}