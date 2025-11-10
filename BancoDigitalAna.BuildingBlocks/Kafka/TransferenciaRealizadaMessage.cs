namespace BancoDigitalAna.BuildingBlocks.Kafka
{
    public record TransferenciaRealizadaMessage (Guid IdRequisicao, Guid ContaCorrenteId, decimal ValorTransferencia, DateTime DataHora)
    {

    }
}
