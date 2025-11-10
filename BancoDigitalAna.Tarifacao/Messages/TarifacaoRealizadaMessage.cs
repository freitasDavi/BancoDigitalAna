namespace BancoDigitalAna.Tarifacao.Messages
{
    public record TarifacaoRealizadaMessage (Guid ContaCorrenteId, decimal ValorTarifado, DateTime DataHora)
    {
    }
}
