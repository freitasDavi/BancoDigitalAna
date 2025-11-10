namespace BancoDigitalAna.BuildingBlocks.Kafka
{
    public record TarifacaoRealizadaMessage (Guid ContaCorrenteId, decimal ValorTarifado, DateTime DataHora)
    {
    }
}
