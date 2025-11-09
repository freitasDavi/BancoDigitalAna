using Asp.Versioning;
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
    //var connectionString = $"User Id={user};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={service}))";
    var connectionString2 = $"User Id={user};Password={password};Data Source={host}:{port}/{service}";
    options.UseOracle(connectionString2);
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine, LogLevel.Information);
    //options.UseSqlite(builder.Configuration.GetConnectionString("default"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContaDbContext>();

    try
    {
        var connection = db.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM CONTACORRENTE";

        var result = await command.ExecuteScalarAsync();
        Console.WriteLine($"V Tabelas encontradas! Count: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"x Erro: {ex.Message}");

        // Tenta com schema
        var connection = db.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT TABLE_NAME, OWNER 
            FROM ALL_TABLES 
            WHERE TABLE_NAME = 'CONTACORRENTE'";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            Console.WriteLine($"Tabela: {reader["TABLE_NAME"]}, Schema: {reader["OWNER"]}");
        }
    }

}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
