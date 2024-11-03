using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GlobalErpData.Models;
using GlobalLib.Database;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalAPINFe.Controllers;
using HotChocolate.AspNetCore;
using HotChocolate.Data;
using HotChocolate.Types;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalErpData.Repository.PagedRepositories;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using System.Globalization;
using GlobalAPINFe.SwaggerUtils;
using GlobalErpData.Services;
using System;
using GlobalAPINFe.Lib;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://217.196.61.51:3339", "http://217.196.61.51:3006", "http://localhost:5127", "http://localhost:5129", "http://localhost:3006")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var Configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760, retainedFileCountLimit: 7)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var y = Configuration["Jwt:Issuer"];
var x = Configuration["Jwt:Audience"];
var z = Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<GlobalErpFiscalBaseContext>(options => options.UseNpgsql(IniFile.GetConnectionString()));

builder.Services.AddScoped<IRepositoryDto<Empresa, int, EmpresaDto>, EmpresaRepositoryDto>();
builder.Services.AddScoped<IRepositoryDto<Cidade, string, CidadeDto>, CidadeRepositoryDto>();
builder.Services.AddScoped<IRepositoryDto<Usuario, string, UsuarioDto>, UsuarioRepositoryDto>();
builder.Services.AddScoped<IRepositoryDto<Permissao, int, PermissaoDto>, PermissaoRepositoryDto>();
builder.Services.AddScoped<IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto>, UsuarioPermissaoRepositoryDto>();
builder.Services.AddScoped<IQueryRepository<ReferenciaEstoque, int, ReferenciaEstoqueDto>, ReferenciaEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<GrupoEstoque, int, GrupoEstoqueDto>, GrupoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutoEstoque, int, ProdutoEstoqueDto>, ProdutoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<UnidadeMedida, int, UnidadeMedidaDto>, UnidadeMedidaPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>, ProdutoEstoquePagedRepositoryMultiKey>();
builder.Services.AddScoped<IQueryRepository<Cliente, int, ClienteDto>, ClientePagedRepositoyDto>();
builder.Services.AddScoped<IQueryRepository<Certificado, int, CertificadoDto>, CertificadoPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Fornecedor, int, int, FornecedorDto>, FornecedorPagedRepositoryMultiKey>();
builder.Services.AddScoped<IQueryRepository<PlanoEstoque, int, PlanoEstoqueDto>, PlanoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepositoryNoCache<SaldoEstoque, int, SaldoEstoqueDto>, SaldoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<CfopImportacao, int, CfopImportacaoDto>, CfopImportacaoPagedRepository>();
builder.Services.AddScoped<IRepositoryDto<Impdupnfe, string, ImpdupnfeDto>, ImpdupnfeRepository>();
builder.Services.AddScoped<IRepositoryDto<Impcabnfe, string, ImpcabnfeDto>, ImpcabnfeRepository>();
builder.Services.AddScoped<IRepositoryDto<Imptotalnfe, string, ImptotalnfeDto>, ImptotalnfeRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Impitensnfe, string, string, ImpitensnfeDto>, ImpitensnfeRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<ConfiguracoesEmpresa, int, string, ConfiguracoesEmpresaDto>, ConfiguracoesEmpresaRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<ConfiguracoesUsuario, int, string, ConfiguracoesUsuarioDto>, ConfiguracoesUsuarioRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutosForn, int, ProdutosFornDto>, ProdutosFornRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto>, ProdutoEntradaRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto>, EntradaPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Transportadora, int, int, TransportadoraDto>, TransportadoraPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<FotosProduto, int, int, FotosProdutoDto>, FotosProdutoRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Featured, int, int, FeaturedDto>, FeaturedRepository>();
builder.Services.AddScoped<IRepositoryDto<Section, int, SectionDto>, SectionRepository>();
builder.Services.AddScoped<IRepositoryDto<SectionItem, int, SectionItemDto>, SectionItemRepository>();
builder.Services.AddScoped<IRepositoryDto<ProductDetail, int, ProductDetailDto>, ProductDetailRepository>();
builder.Services.AddScoped<IRepositoryDto<ItemDetail, int, ItemDetailDto>, ItemDetailRepository>();
builder.Services.AddScoped<IRepositoryDto<PerfilLoja, int, PerfilLojaDto>, PerfilLojaRepository>();
builder.Services.AddScoped<IRepositoryDto<OlderItem, Guid, OlderItemDto>, OlderItemRepository>();
builder.Services.AddScoped<IQueryRepository<Older, Guid, OlderDto>, OlderRepository>();
builder.Services.AddScoped<IRepositoryDto<Category, int, CategoryDto>, CategoryRep>();
builder.Services.AddScoped<IQueryRepository<ContaDoCaixa, int, ContaCaixaDto>, ContaCaixaRepository>();
builder.Services.AddScoped<IQueryRepository<PlanoDeCaixa, int, PlanoCaixaDto>, PlanoCaixaRepository>();
builder.Services.AddScoped<IQueryRepository<HistoricoCaixa, int, HistoricoCaixaDto>, HistoricoCaixaRepository>();
builder.Services.AddScoped<IQueryRepository<FormaPagt, int, FormaPagtDto>, FormaPagtRepository>();
builder.Services.AddScoped<IQueryRepository<ContasAPagar, int, ContasAPagarDto>, ContasAPagarRepository>();
builder.Services.AddScoped<IQueryRepository<Saida, int, SaidaDto>, SaidaRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutoSaidum, int, ProdutoSaidumDto>, ProdutoSaidumRepository>();
builder.Services.AddScoped<IQueryRepository<ContasAReceber, int, ContasAReceberDto>, ContasAReceberRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Impxml, int, string, ImpxmlDto>, ImpXmlRepository>();
builder.Services.AddScoped<EntradaCalculationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GlobalErpFiscalBaseContext>();
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao inicializar o banco de dados.");
    }
}

app.Run();