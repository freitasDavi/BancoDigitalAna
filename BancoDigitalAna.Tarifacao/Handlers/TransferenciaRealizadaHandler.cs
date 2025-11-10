using BancoDigitalAna.BuildingBlocks.Kafka;
using BancoDigitalAna.Tarifacao.Domain.Entities;
using BancoDigitalAna.Tarifacao.Domain.Repositories;
using BancoDigitalAna.Tarifacao.Producers;
using KafkaFlow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BancoDigitalAna.Tarifacao.Handlers
{
    public class TransferenciaRealizadaHandler : IMessageHandler<TransferenciaRealizadaMessage>
    {
        private readonly ITarifacaoRepository _tarifacaoRepository;
        private readonly ITarifacaoProducer _tarifacaoProducer;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TransferenciaRealizadaHandler> _logger;

        public TransferenciaRealizadaHandler(ITarifacaoRepository tarifacaoRepository, ITarifacaoProducer producer, IConfiguration configuration, ILogger<TransferenciaRealizadaHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _tarifacaoProducer = producer;
            _tarifacaoRepository = tarifacaoRepository;
        }

        public async Task Handle(IMessageContext context, TransferenciaRealizadaMessage message)
        {
            try
            {
                _logger.LogInformation($"[INFO]: Processando tarifação para transferência {message.IdRequisicao} da conta {message.ContaCorrenteId}");

                var valorTarifa = _configuration.GetValue<decimal>("Tarifacao:ValorTarifa");

                var tarifa = Tarifas.Criar(message.IdRequisicao, valorTarifa);

                await _tarifacaoRepository.Inserir(tarifa);

                _logger.LogInformation($"[INFO]: Tarifa com {tarifa.Id} registrada com sucesso com o valor de {valorTarifa}");

                var tarifacaoMessage = new TarifacaoRealizadaMessage(message.ContaCorrenteId, valorTarifa, tarifa.DataHora);

                await _tarifacaoProducer.PublicarTarifacaoRealizada(tarifacaoMessage);

                _logger.LogInformation($"[INFO]: Tarifa publicada no tópico para conta {message.ContaCorrenteId}");

            } catch (Exception ex)
            {
                _logger.LogError(ex, $"[ERROR]: Erro ao processar tarifação para transferência {message.IdRequisicao}");

                throw;
            }
        }
    }
}
