using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.BuildingBlocks.Middlewares;
using BancoDigitalAna.Transferencia.Infrastructure;
using KafkaFlow;
using KafkaFlow.Serializer;

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
        Title = "Banco Digital da Ana - Transferência",
        Description = "Api para transferência de dinheiro entre contas no Banco Digital da Ana",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Davi Freitas da Silva",
            Url = new Uri("https://linkedin.com/in/freitasDavi")
        }
    });
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddKafka(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers(new[] { builder.Configuration["Kafka:BootstrapServers"]! })
        .AddProducer("transferencia-producer", producer => producer
            .DefaultTopic("transferencias-realizadas")
            .AddMiddlewares(middlewares => middlewares
                .AddSerializer<JsonCoreSerializer>()
            )
        )
    )
);

DependencyInjection.AddCoreServices(builder.Services);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    //config.AddOpenBehavior(typeof());
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
