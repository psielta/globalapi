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
        .WithOrigins("http://200.98.160.51:8104", "http://145.223.29.182:3006", "http://localhost:5127", "http://localhost:5129", "http://localhost:3006", "http://46.202.151.238:3006")            
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

builder.Services.AddPooledDbContextFactory<GlobalErpFiscalBaseContext>(options =>
    options.UseNpgsql(IniFile.GetConnectionString()));

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<GlobalErpFiscalBaseContext>(DbContextKind.Pooled)
    .AddQueryType<GlobalNfeGraphql.GraphQL.Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();
