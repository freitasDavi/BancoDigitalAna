using BancoDigitalAna.BuildingBlocks.Kafka;

namespace BancoDigitalAna.Transferencia.Infrastructure.Producers
{
    public interface ITransferenciaProducer
    {
        Task PublicarTransferenciaRealizada(TransferenciaRealizadaMessage message);
    }
}
