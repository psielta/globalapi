using Microsoft.EntityFrameworkCore;
using GlobalErpData.Data;
using GlobalErpData.Models;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GlobalErpData.Dto;
using GlobalErpData.Repository.PagedRepositories;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalErpData.Repository.Repositories;
using GlobalErpData.Repository;
using GlobalAPI_ACBrNFe.Lib.ACBr.Web;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Services;
using GlobalErpData.Identity;
using Microsoft.AspNetCore.Identity;
using GlobalLib.Repository;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
using GlobalAPI_ACBrNFe.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowSpecificOrigin",
        builder => builder
         .WithOrigins("http://200.98.160.51:8104", "http://145.223.29.182:3006", "http://localhost:5127", "http://localhost:5129", "http://localhost:3006", "http://46.202.151.238:3006")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var Configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760, retainedFileCountLimit : 7)
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

// Add services to the container.
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
builder.Services.AddScoped<EntradaCalculationService>();
builder.Services.AddScoped<NFeGlobalService>();
builder.Services.AddScoped<SaidaCalculationService>();
builder.Services.AddScoped<SintegraService>();

builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<IUserStore<Usuario>, CustomUserStore>();
//builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, CustomPasswordHasher>();
builder.Services.AddHostedService<DistribuicaoDFeService>();
builder.Services.AddIdentityCore<Usuario>(options =>
{
    // Configurações de senha
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddSignInManager<SignInManager<Usuario>>() // Adicionado para registrar o SignInManager
.AddUserStore<CustomUserStore>()
.AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddScoped<IRepository<TblAccessToken>, TblAccessTokenRepository>();
//builder.Services.AddHostedService<AtualizarTokenService>();
builder.Services.AddACBrNFe(o => o.UseMemory = false);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicione o middleware de roteamento
app.UseRouting();

// Configure o CORS antes dos middlewares de autenticação e autorização
app.UseCors("AllowSpecificOrigin");
//app.UseCors("PermitirTodos");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores dentro do middleware de endpoints
app.MapControllers();
app.MapHub<ImportProgressHub>("/importProgressHub");

app.Run();