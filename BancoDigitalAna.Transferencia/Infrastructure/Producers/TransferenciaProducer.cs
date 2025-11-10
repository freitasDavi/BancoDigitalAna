using BancoDigitalAna.BuildingBlocks.Kafka;
using KafkaFlow.Producers;

namespace BancoDigitalAna.Transferencia.Infrastructure.Producers
{
    public class TransferenciaProducer : ITransferenciaProducer
    {
        private readonly IProducerAccessor _producerAccessor;
        private readonly ILogger<TransferenciaProducer> _logger;

        public TransferenciaProducer(IProducerAccessor producerAccessor, ILogger<TransferenciaProducer> logger)
        {
            _logger = logger;
            _producerAccessor = producerAccessor;
        }
        public async Task PublicarTransferenciaRealizada(TransferenciaRealizadaMessage message)
        {
            var producer = _producerAccessor.GetProducer("transferencia-producer");

            await producer.ProduceAsync(
                    message.IdRequisicao,
                    message
                );

            _logger.LogInformation($"[INFO]: Transferência {message.IdRequisicao} publicada no Kafka");
        }
    }
}
