using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.BuildingBlocks.Middlewares;
using BancoDigitalAna.Conta.Application.Handlers;
using BancoDigitalAna.Conta.Infrastructure.Database;
using BancoDigitalAna.Conta.Infrastructure.Services;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Banco Digital da Ana - Conta Corrente",
        Description = "Api para criação de contas e movimentação de dinheiro do Banco Digital da Ana",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Davi Freitas da Silva",
            Url = new Uri("https://linkedin.com/in/freitasDavi")
        }
    });
});

// Adicionando Autenticação
builder.Services.AddJwtAuthentication(builder.Configuration);

// Adicionando Serviços base
DependencyInjection.AddCoreServices(builder.Services);

// Adicionando Mediator
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});

// Adicionando Versionamento da Api
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; 
});

// Adicionando Kafka
builder.Services.AddKafka(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers(new[] { builder.Configuration["Kafka:BootstrapServers"]! })
        .AddConsumer(consumer => consumer
            .Topic("tarifacoes-realizadas")
            .WithGroupId("contacorrente-service-group")
            .WithBufferSize(100)
            .WithWorkersCount(10)
            .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Earliest)
            .AddMiddlewares(middlewares => middlewares
                .AddDeserializer<KafkaFlow.Serializer.JsonCoreDeserializer>()
                .AddTypedHandlers(handlers => handlers
                    .AddHandler<TarifacaoRealizadaHandler>()
                )
            )
        )
    )
);

var host = Environment.GetEnvironmentVariable("ORACLE_HOST") ?? builder.Configuration.GetValue<string>("ORACLE_HOST");
var port = Environment.GetEnvironmentVariable("ORACLE_PORT") ?? builder.Configuration.GetValue<string>("ORACLE_PORT"); 
var user = Environment.GetEnvironmentVariable("ORACLE_USER") ?? builder.Configuration.GetValue<string>("ORACLE_USER"); 
var password = Environment.GetEnvironmentVariable("ORACLE_PASSWORD") ?? builder.Configuration.GetValue<string>("ORACLE_PASSWORD");
var service = Environment.GetEnvironmentVariable("ORACLE_SERVICE") ?? builder.Configuration.GetValue<string>("ORACLE_SERVICE");

// Conexão com o banco de dados
builder.Services.AddDbContext<ContaDbContext>(options =>
{
    var connectionString = $"User Id={user};Password={password};Data Source={host}:{port}/{service}";
    options.UseOracle(connectionString);
    options.LogTo(Console.WriteLine, LogLevel.Information);
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

var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();

app.Run();

await kafkaBus.StopAsync();