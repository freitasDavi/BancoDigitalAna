
using BancoDigitalAna.Tarifacao.Domain.Repositories;
using BancoDigitalAna.Tarifacao.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BancoDigitalAna.Tarifacao.Handlers;
using KafkaFlow;
using KafkaFlow.Serializer;
using BancoDigitalAna.Tarifacao.Producers;
class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<ITarifacaoProducer, TarifacaoProducer>();
                services.AddScoped<ITarifacaoRepository, TarifacaoRepository>();

                var kafkaBootstrap = context.Configuration["Kafka:BootstrapServers"];

                services.AddKafka(kafka => kafka
                    .UseConsoleLog()
                    .AddCluster(cluster => cluster
                        .WithBrokers(new[] { kafkaBootstrap! })
                        .AddConsumer(consumer => consumer
                            .Topic("transferencias-realizadas")
                            .WithGroupId("tarifa-service-group")
                            .WithBufferSize(100)
                            .WithWorkersCount(10)
                            .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Earliest)
                            .AddMiddlewares(middlewares => middlewares
                                .AddDeserializer<KafkaFlow.Serializer.JsonCoreDeserializer>()
                                .AddTypedHandlers(handlers => handlers
                                .AddHandler<TransferenciaRealizadaHandler>()
                                )
                            )
                        )
                        .AddProducer("tarifacao-producer", producer => producer
                            .DefaultTopic("tarifacoes-realizadas")
                            .AddMiddlewares(middlewares => middlewares
                                .AddSerializer<JsonCoreSerializer>()
                             )
                        )
                    )
                );
            })
            .Build();

        var kafkaBus = host.Services.CreateKafkaBus();
        await kafkaBus.StartAsync();

        await host.RunAsync();

        await kafkaBus.StopAsync();
    }
}