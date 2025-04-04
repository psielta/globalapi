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
            .WithOrigins("http://192.168.1.102:3006", "http://192.168.1.131:3006", "http://192.168.1.134:3006", "http://192.168.1.100:3006", "http://192.168.1.101:3006", "http://200.98.160.51:8104", "http://145.223.29.182:3006", "http://localhost:5127", "http://localhost:5129", "http://localhost:3006", "http://46.202.151.238:3006", "http://191.101.235.12:3006")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
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
builder.Services.AddScoped<IRepositoryDto<Csosn, string, CsosnDto>, CsosnRepository>();
builder.Services.AddScoped<IRepositoryDto<TipoNf, string, TipoNfDto>, TipoNfRepository>();
builder.Services.AddScoped<IRepositoryDto<Cfop, string, CfopDto>, CfopRepository>();
builder.Services.AddScoped<IRepositoryDto<Cst, string, CstDto>, CstRepository>();
builder.Services.AddScoped<IRepositoryDto<OrigemCst, string, OrigemCstDto>, OrigemCstRepository>();
builder.Services.AddScoped<IRepositoryDto<CestNcm, int, CestNcmDto>, CestNcmRepository>();
builder.Services.AddScoped<IRepositoryDto<Ncm, int, NcmDto>, NcmRepository>();
builder.Services.AddScoped<IRepositoryDto<Ibpt, int, IbptDto>, IbptRepository>();
builder.Services.AddScoped<IQueryRepository<ReferenciaEstoque, int, ReferenciaEstoqueDto>, ReferenciaEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<GrupoEstoque, int, GrupoEstoqueDto>, GrupoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutoEstoque, int, ProdutoEstoqueDto>, ProdutoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<UnidadeMedida, int, UnidadeMedidaDto>, UnidadeMedidaPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>, ProdutoEstoquePagedRepositoryMultiKey>();
builder.Services.AddScoped<IQueryRepository<Cliente, int, ClienteDto>, ClientePagedRepositoyDto>();
builder.Services.AddScoped<IQueryRepository<Certificado, int, CertificadoDto>, CertificadoPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Fornecedor, int, int, FornecedorDto>, FornecedorPagedRepositoryMultiKey>();
builder.Services.AddScoped<IQueryRepository<PlanoEstoque, int, PlanoEstoqueDto>, PlanoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<SaldoEstoque, int, SaldoEstoqueDto>, SaldoEstoquePagedRepository>();
builder.Services.AddScoped<IQueryRepository<CfopImportacao, int, CfopImportacaoDto>, CfopImportacaoPagedRepository>();
builder.Services.AddScoped<IRepositoryDto<Impdupnfe, string, ImpdupnfeDto>, ImpdupnfeRepository>();
builder.Services.AddScoped<IRepositoryDto<Impcabnfe, string, ImpcabnfeDto>, ImpcabnfeRepository>();
builder.Services.AddScoped<IRepositoryDto<Imptotalnfe, string, ImptotalnfeDto>, ImptotalnfeRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Impitensnfe, string, string, ImpitensnfeDto>, ImpitensnfeRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<ConfiguracoesEmpresa, int, string, ConfiguracoesEmpresaDto>, ConfiguracoesEmpresaRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<ConfiguracoesUsuario, int, string, ConfiguracoesUsuarioDto>, ConfiguracoesUsuarioRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<UsuarioFuncionario, int, string, UsuarioFuncionarioDto>, UsuarioFuncionarioRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Vendedor, int, int, VendedorDto>, VendedorRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Funcionario, int, int, FuncionarioDto>, FuncionarioRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutosForn, int, ProdutosFornDto>, ProdutosFornRepository>();
builder.Services.AddScoped<IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto>, ProdutoEntradaRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto>, EntradaPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Transportadora, int, int, TransportadoraDto>, TransportadoraPagedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<FotosProduto, int, int, FotosProdutoDto>, FotosProdutoRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<Featured, int, int, FeaturedDto>, FeaturedRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<NfceFormaPgt, int, int, NfceFormaPgtDto>, NfceFormaPgtRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<NfceProdutoSaidum, int, int, NfceProdutoSaidumDto>, NfceProdutoSaidumRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<NfceSaida, int, int, NfceSaidaDto>, NfceSaidaRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto>, NfceAberturaCaixaRepository>();
builder.Services.AddScoped<IQueryRepositoryMultiKey<SangriaCaixa, int, int, SangriaCaixaDto>, SangriaCaixaRepository>();
builder.Services.AddScoped<IRepositoryDto<Section, int, SectionDto>, SectionRepository>();
builder.Services.AddScoped<IRepositoryDto<SectionItem, int, SectionItemDto>, SectionItemRepository>();
builder.Services.AddScoped<IRepositoryDto<ProductDetail, int, ProductDetailDto>, ProductDetailRepository>();
builder.Services.AddScoped<IRepositoryDto<ItemDetail, int, ItemDetailDto>, ItemDetailRepository>();
builder.Services.AddScoped<IRepositoryDto<PerfilLoja, int, PerfilLojaDto>, PerfilLojaRepository>();
builder.Services.AddScoped<IRepositoryDto<OlderItem, Guid, OlderItemDto>, OlderItemRepository>();
builder.Services.AddScoped<IRepositoryDto<Unity, int, UnityDto>, UnityRepository>();
builder.Services.AddScoped<IRepositoryDto<UsuarioEmpresa, int, UsuarioEmpresaDto>, UsuarioEmpresaRepository>();
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
builder.Services.AddScoped<IQueryRepository<ObsNf, int, ObsNfDto>, ObsNfRepository>();
builder.Services.AddScoped<IQueryRepository<Frete, int, FreteDto>, FreteRepository>();
builder.Services.AddScoped<IQueryRepository<SaidasVolume, int, SaidasVolumeDto>, SaidasVolumeRepository>();
builder.Services.AddScoped<IQueryRepository<SaidaNotasDevolucao, int, SaidaNotasDevolucaoDto>, SaidaNotasDevolucaoRepository>();
builder.Services.AddScoped<IQueryRepository<ProtocoloEstadoNcm, int, ProtocoloEstadoNcmDto>, ProtocoloEstadoNcmRepository>();
builder.Services.AddScoped<IQueryRepository<CfopCsosnV2, int, CfopCsosnV2Dto>, CfopCsosnV2Repository>();
builder.Services.AddScoped<IQueryRepository<NcmProtocoloEstado, int, NcmProtocoloEstadoDto>, NcmProtocoloEstadoRepository>();
builder.Services.AddScoped<IQueryRepository<Icm, int, IcmDto>, IcmRepository>();
builder.Services.AddScoped<IQueryRepository<ControleNumeracaoNfe, int, ControleNumeracaoNfeDto>, ControleNumeracaoNfeRepository>();
builder.Services.AddScoped<IQueryRepository<RetiradaNfe, int, RetiradaNfeDto>, RetiradaNfeRepository>();
builder.Services.AddScoped<IQueryRepository<EntregaNfe, int, EntregaNfeDto>, EntregaNfeRepository>();
builder.Services.AddScoped<IQueryRepository<LivroCaixa, long, LivroCaixaDto>, LivroCaixaRepository>();
builder.Services.AddScoped<IQueryRepository<PagtosParciaisCr, int, PagtosParciaisCrDto>, PagtosParciaisCrRepository>();
builder.Services.AddScoped<IQueryRepository<PagtosParciaisCp, int, PagtosParciaisCpDto>, PagtosParciaisCpRepository>();
builder.Services.AddScoped<IQueryRepository<DistribuicaoDfe, Guid, DistribuicaoDfeDto>, DistribuicaoDfeRepository>();
builder.Services.AddScoped<IQueryRepository<EntradaOutrasDesp, int, EntradaOutrasDespDto>, EntradaOutrasDespRepository>();
builder.Services.AddScoped<IQueryRepository<Cte, int, CteDto>, CteRepository>();
builder.Services.AddScoped<EntradaCalculationService>();
builder.Services.AddScoped<NFeGlobalService>();
builder.Services.AddScoped<SaidaCalculationService>();
builder.Services.AddScoped<SintegraService>();
builder.Services.AddScoped<ProdutoSaidumService>();

builder.Services.AddScoped<EntradaCalculationService>();
builder.Services.AddScoped<SaidaCalculationService>();

builder.Services.AddScoped<BaixaCRService>();
builder.Services.AddScoped<BaixaCPService>();

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

builder.Services.AddACBrNFe(o => o.UseMemory = false);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


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

app.MapControllers();
app.MapHub<ImportProgressHub>("/importProgressHub");

app.Run();