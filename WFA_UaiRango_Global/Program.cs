using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using GlobalErpData.Models;
using Serilog;
using Microsoft.Extensions.Configuration;
using WFA_UaiRango_Global.Services.Culinaria;
using GlobalErpData.Data;
using WFA_UaiRango_Global.Services.Login;
using WFA_UaiRango_Global.Services.Estabelecimentos;


namespace WFA_UaiRango_Global
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            services.AddDbContext<GlobalErpFiscalBaseContext>(options =>
                options.UseNpgsql(IniFile.GetConnectionString()));

            // Registrar serviços


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/uairango.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10_485_760, // 10MB
                    retainedFileCountLimit: 7)
                .CreateLogger();
            Log.Information("Aplicativo iniciando...");
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(Log.Logger, dispose: true);
            });

            var configuration = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddHttpClient();

            #region Services
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ICulinariaService, CulinariaService>();
            services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
            #endregion

            var provider = services.BuildServiceProvider();
            Application.Run(ActivatorUtilities.CreateInstance<MainForm>(provider));
        }
    }
}