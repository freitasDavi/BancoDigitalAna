using BancoDigitalAna.Tarifacao.Messages;
using KafkaFlow.Producers;
using Microsoft.Extensions.Logging;

namespace BancoDigitalAna.Tarifacao.Producers
{
    public class TarifacaoProducer : ITarifacaoProducer
    {
        private readonly IProducerAccessor _producerAccessor;
        private readonly ILogger<TarifacaoProducer> _logger;

        public TarifacaoProducer(IProducerAccessor producerAccessor, ILogger<TarifacaoProducer> logger)
        {
            _logger = logger;
            _producerAccessor = producerAccessor;
        }

        public async Task PublicarTarifacaoRealizada(TarifacaoRealizadaMessage message)
        {
            var producer = _producerAccessor.GetProducer("tarifacao-producer");

            await producer.ProduceAsync(
                message.ContaCorrenteId,
                message
                );

            _logger.LogInformation($"[INFO]: Tarifação publicada para conta {message.ContaCorrenteId} com o valor de {message.ValorTarifado}");
        }
    }
}
