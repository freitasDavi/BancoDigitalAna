using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.BuildingBlocks.Middlewares;
using BancoDigitalAna.Conta.Infrastructure.Database;
using BancoDigitalAna.Conta.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication(builder.Configuration);

DependencyInjection.AddCoreServices(builder.Services);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    //config.AddOpenBehavior(typeof());
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; 
});

var host = Environment.GetEnvironmentVariable("ORACLE_HOST") ?? builder.Configuration.GetValue<string>("ORACLE_HOST");
var port = Environment.GetEnvironmentVariable("ORACLE_PORT") ?? builder.Configuration.GetValue<string>("ORACLE_PORT"); 
var user = Environment.GetEnvironmentVariable("ORACLE_USER") ?? builder.Configuration.GetValue<string>("ORACLE_USER"); 
var password = Environment.GetEnvironmentVariable("ORACLE_PASSWORD") ?? builder.Configuration.GetValue<string>("ORACLE_PASSWORD");
var service = Environment.GetEnvironmentVariable("ORACLE_SERVICE") ?? builder.Configuration.GetValue<string>("ORACLE_SERVICE");


builder.Services.AddDbContext<ContaDbContext>(options =>
{
    var connectionString2 = $"User Id={user};Password={password};Data Source={host}:{port}/{service}";
    options.UseOracle(connectionString2);
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine, LogLevel.Information);
    //options.UseSqlite(builder.Configuration.GetConnectionString("default"));
});

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
