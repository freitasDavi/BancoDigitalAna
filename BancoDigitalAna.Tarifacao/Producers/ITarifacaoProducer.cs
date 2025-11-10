using BancoDigitalAna.BuildingBlocks.Kafka;

namespace BancoDigitalAna.Tarifacao.Producers
{
    public interface ITarifacaoProducer
    {
        Task PublicarTarifacaoRealizada(TarifacaoRealizadaMessage message);
    }
}
