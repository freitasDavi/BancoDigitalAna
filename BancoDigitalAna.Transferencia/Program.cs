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
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication(builder.Configuration);

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
