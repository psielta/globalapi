using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GlobalErpData.Repository;
using GlobalErpData.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using GlobalNfeGraphql.GraphQL;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalErpData.Repository.Repositories;
using GlobalErpData.Services;

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

// Add services to the container.
//builder.Services.AddDbContext<GlobalErpContext>(options => options.UseNpgsql(IniFile.GetConnectionString()));

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddDbContext<GlobalErpFiscalBaseContext>(options =>
//    options.UseNpgsql(IniFile.GetConnectionString()));

// Adiciona a f�brica de DbContext para cria��o sob demanda
builder.Services.AddPooledDbContextFactory<GlobalErpFiscalBaseContext>(options =>
    options.UseNpgsql(IniFile.GetConnectionString()));

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<GlobalErpFiscalBaseContext>(DbContextKind.Pooled)
    .AddQueryType<GlobalNfeGraphql.GraphQL.Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();



//builder.Services.AddScoped<IRepository<TblAccessToken>, TblAccessTokenRepository>();
//builder.Services.AddHostedService<AtualizarTokenService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();