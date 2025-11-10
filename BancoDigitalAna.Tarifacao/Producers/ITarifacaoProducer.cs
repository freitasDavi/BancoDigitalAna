using BancoDigitalAna.Tarifacao.Messages;

namespace BancoDigitalAna.Tarifacao.Producers
{
    public interface ITarifacaoProducer
    {
        Task PublicarTarifacaoRealizada(TarifacaoRealizadaMessage message);
    }
}
